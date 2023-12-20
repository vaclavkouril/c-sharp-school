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

            try{
                var tree = HuffmanTreeBuilder.BuildTree(args[0]);
                tree.PrintTree();
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
        public Dictionary<byte, int> ReadFile(string filePath)
        {
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

        public void PrintTree(){
            PrintNode(_root);
        }

        private void PrintNode(TreeNode node){
            if (node == null) return;

            if (node.Symbol.HasValue)
            {
                Console.Write($"*{node.Symbol.Value}:{node.Weight} ");
            }
            else
            {
                Console.Write($"{node.Weight} ");
                PrintNode(node.LeftChild);
                PrintNode(node.RightChild);
            }
        }
    }
}
