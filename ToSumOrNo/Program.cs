using System; 
using System.Collections.Generic;
using System.IO;
using System.Linq;



namespace ToSumOrNo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3){
                Console.WriteLine("Argument Error");
                return;
            }

            string input = args[0];
            string output = args[1];
            string columnName = args[2];

            if (!File.Exists(input)){
                Console.WriteLine("File Error");
                return;
            }

            long? sum = SumColumn(input,columnName);
            if (sum != null){
                Output(output, columnName, sum);
            }
            
        }



        public static long? SumColumn(string inputFile, string nameOfColumn){
            int numOfWordsOnFirstLine = 0;
            int numOfWordsOnLine = 0;
            int searchedIndex = 0;
            bool firstLineLoad = true;
            long sum = 0;
            string word = "";
            bool insideWord = false;
            using (StreamReader reader = new StreamReader(inputFile)){
                int charInt;
		while ((charInt = reader.Read())!= -1){
		    char character = (char)charInt;
                    if(!char.IsWhiteSpace(character)){
                        insideWord = true;
		        word += character;
		    }
		    else if(char.IsWhiteSpace(character) && insideWord){
                        numOfWordsOnLine++;
                        if (firstLineLoad && searchedIndex == 0){
                            if (word == nameOfColumn){
                                searchedIndex = numOfWordsOnLine;
                            }
                        }
                        else if (numOfWordsOnLine == searchedIndex){
                            try{
                                sum += Convert.ToInt32(word);
                            }
                            catch{
                                Console.WriteLine("Invalid Integer Value");
                                return null;
                            }
                        }
                        word = "";
                        insideWord = false;
					}
                    if(character=='\n'){
                        if (firstLineLoad){
                            firstLineLoad = false;
                            numOfWordsOnFirstLine = numOfWordsOnLine;
                            if (searchedIndex == 0){
                                if (numOfWordsOnLine == 0) Console.WriteLine("Invalid File Format");
                                else Console.WriteLine("Non-existent Column Name");
                            return null;
                            }
                        }
                        if (numOfWordsOnFirstLine != numOfWordsOnLine){
                            Console.WriteLine("Invalid File Format");
                            return null;
                        }
                        numOfWordsOnLine = 0;
                        
                    }
                }
                if (word != "" || numOfWordsOnLine != 0){
                    char character = '\n';
					if(char.IsWhiteSpace(character) && insideWord){
                        numOfWordsOnLine++;
                        if (firstLineLoad && searchedIndex == 0){
                            if (word == nameOfColumn){
                                searchedIndex = numOfWordsOnLine;
                            }
                        }
                        else if (numOfWordsOnLine == searchedIndex){
                            try{
                                sum += Convert.ToInt32(word);
                            }
                            catch{
                                Console.WriteLine("Invalid Integer Value");
                                return null;
                            }
                        }
                        word = "";
                        insideWord = false;
					}
                    if (firstLineLoad){
                        firstLineLoad = false;
                        numOfWordsOnFirstLine = numOfWordsOnLine;
                        if (searchedIndex == 0){
                            if (numOfWordsOnLine == 0) Console.WriteLine("Invalid File Format");
                                else Console.WriteLine("Non-existent Column Name");
                            return null;
                            }
                    }
                    if (numOfWordsOnFirstLine != numOfWordsOnLine){
                        Console.WriteLine("Invalid File Format");
                        return null;
                    }
                }

            
            return sum;
            }
        }
        public static void Output(string outputFileName, string nameOfColumn, long? result){
            try{
                using ( StreamWriter writer = new StreamWriter(outputFileName)){
                    writer.WriteLine(nameOfColumn);
                    foreach(char c in nameOfColumn){
                        writer.Write("-");
                    }
                    writer.WriteLine();
                    writer.WriteLine(result.ToString());


                }   
            }
            catch{
                Console.WriteLine("File Error");
            }

        }
    }
}

