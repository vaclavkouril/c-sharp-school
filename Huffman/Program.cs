using System;
using System.IO;
using System.Collections.Generic;

namespace Huffman 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1){
                Console.WriteLine("Argument Error");
                return;
            }
            string inputFilePath = args[0];
            string outputFilePath = inputFilePath + ".huff";
            
            try{
                var tree = HuffmanTreeBuilder.BuildTree(inputFilePath);
                var encodingTable = tree.GenerateEncodingTable();
                using (var inputFileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                using (var outputFileStream = new FileStream(outputFilePath, FileMode.Create))
                using (var binaryWriter = new BinaryWriter(outputFileStream)){
                    tree.HeaderToStream(binaryWriter);             
                    tree.WriteTreeToStream(binaryWriter);
                    Encoder.EncodeData(encodingTable, inputFileStream, binaryWriter);
                }            
            }
            catch{
                Console.WriteLine("File Error");
            }
        }
    }

    class HuffmanTreeBuilder{
        public static HuffmanTree BuildTree(string filePath){
            var fileReader = new FileReader();
            var frequencies = fileReader.ReadFile(filePath);
            var priorityQueue = new PriorityQueue();

            foreach (var entry in frequencies)
                priorityQueue.Enqueue(new TreeNode(entry.Key, entry.Value));
        
            int age = 1;
            while (priorityQueue.Count > 1){
                var left = priorityQueue.Dequeue();
                var right = priorityQueue.Dequeue();
                var parent = new TreeNode(left.Weight + right.Weight, age , left, right);
                priorityQueue.Enqueue(parent);
                age++;
            }

            return new HuffmanTree(priorityQueue.Dequeue());
        }
    }

    class PriorityQueue{
        private List<TreeNode> _elements = new List<TreeNode>();

        public int Count => _elements.Count;

        public void Enqueue(TreeNode node){
            _elements.Add(node);
            _elements.Sort();
        }

        public TreeNode Dequeue(){
            TreeNode node = _elements[0];
            _elements.RemoveAt(0);
            return node;
        }
    }



    class FileReader{
        public Dictionary<byte, int> ReadFile(string filePath){
            var frequencies = new Dictionary<byte, int>();
            
            using (FileStream fileStream = File.OpenRead(filePath)){
                int currentByte;
                while ((currentByte = fileStream.ReadByte()) != -1){
                    byte b = (byte)currentByte;
                    if (!frequencies.ContainsKey(b))
                        frequencies[b] = 0;
                    frequencies[b]++;
                }
            }
            return frequencies;
        }
    }


    class TreeNode : IComparable<TreeNode>{
        public long Weight { get; }
        public byte? Symbol { get; }
        public int Age { get; }
        public TreeNode LeftChild { get; }
        public TreeNode RightChild { get; }

        public TreeNode(byte symbol, long weight){
            Symbol = symbol;
            Weight = weight;
            Age = 0;
        }

        public TreeNode(long weight, int age, TreeNode leftChild, TreeNode rightChild){
            Weight = weight;
            LeftChild = leftChild;
            RightChild = rightChild;
            Age = age;
        }

        public int CompareTo(TreeNode? other){
            if (other == null) return 1;

            int weightComparison = Weight.CompareTo(other.Weight);
            if (weightComparison != 0) return weightComparison;
            
            bool isLeaf = Symbol.HasValue;
            bool otherIsLeaf = other.Symbol.HasValue;

            if (isLeaf && otherIsLeaf) 
                return Symbol.Value.CompareTo(other.Symbol.Value);

            if (isLeaf && !otherIsLeaf) return -1;
            if (!isLeaf && otherIsLeaf) return 1;
            return Age.CompareTo(other.Age);
        }    
    }

    class HuffmanTree{
        private TreeNode _root;

        public HuffmanTree(TreeNode root){
            _root = root;
        }


        public void HeaderToStream(BinaryWriter writer){
            byte[] header = { 0x7B, 0x68, 0x75, 0x7C, 0x6D, 0x7D, 0x66, 0x66 };
            writer.Write(header);

        }

        public Dictionary<byte, string> GenerateEncodingTable(){
            var table = new Dictionary<byte, string>();
            GenerateEncodingTable(_root, "", table);
            return table;
        }

        private void GenerateEncodingTable(TreeNode node, string code, Dictionary<byte, string> table){
            if (node == null) return;

            if (node.Symbol.HasValue){
                table[node.Symbol.Value] = code;
            }
            else{
                GenerateEncodingTable(node.LeftChild, code + "0", table);
                GenerateEncodingTable(node.RightChild, code + "1", table); 
            }
        }


        public void PrintTree(){
            PrintNode(_root);
        }

        private void PrintNode(TreeNode node){
            if (node == null) return;

            if (node.Symbol.HasValue){
                Console.Write($"*{node.Symbol.Value}:{node.Weight} ");
            }
            else{
                Console.Write($"{node.Weight} ");
                PrintNode(node.LeftChild);
                PrintNode(node.RightChild);
            }
        }

        public void WriteTreeToStream(BinaryWriter writer){
            WriteNodeToStream(_root, writer);
            writer.Write(new byte[8]); 
        }

        private void WriteNodeToStream(TreeNode node, BinaryWriter writer){
            if (node == null) return;

            ulong data = 0;
            if (node.Symbol.HasValue){
                data = 1;
                data |= ((ulong)node.Weight & 0x00FFFFFFFFFFFFFF) << 1;
                data |= (ulong)node.Symbol.Value << 56;
            }
            else{
                data = ((ulong)node.Weight & 0x00FFFFFFFFFFFFFF) << 1;
            }
            writer.Write(data);
            
            WriteNodeToStream(node.LeftChild, writer);
            WriteNodeToStream(node.RightChild, writer);
        }
    }

    
    
    class Encoder
    {
        public static void EncodeData(Dictionary<byte, string> encodingTable, Stream inputStream, BinaryWriter outputWriter){
            int currentByte;
            int bitBuffer = 0;
            int bitBufferLength = 0;

            while ((currentByte = inputStream.ReadByte()) != -1){
                string code = encodingTable[(byte)currentByte];

                foreach (char bit in code){
                    bitBuffer = (bitBuffer << 1) | (bit == '1' ? 1 : 0);
                    bitBufferLength++;

                    if (bitBufferLength == 8){
                        outputWriter.Write((byte)ReverseBits(bitBuffer));
                        bitBuffer = 0;
                        bitBufferLength = 0;
                    }
                }
            }

            if (bitBufferLength > 0){
                bitBuffer <<= (8 - bitBufferLength);
                outputWriter.Write(ReverseBits(bitBuffer));
            }
        }

        private static byte ReverseBits(int b){
            int reversed = 0;
            for (int i = 0; i < 8; i++){
                reversed = (reversed << 1) | (b & 1);
                b >>= 1;
            }
            return (byte)reversed;
        }
    }
}
