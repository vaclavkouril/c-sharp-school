using System; 
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordCounting
{
	class FileLoader{
		public WordCounter count;
		public FileLoader(string filename){
			count = new WordCounter();
			bool insideWord = false;
			bool firstNewLine = false;
			string word = "";
			using (StreamReader reader = new StreamReader(filename))
			{
				int charInt;
				while ((charInt = reader.Read())!= -1){
					
					char character = (char)charInt;
					if(!char.IsWhiteSpace(character)){
						insideWord=true;
						word+=character;
					}
					else if(char.IsWhiteSpace(character) && insideWord){
						count.Add();
						// count.Add(word);
						word = "";
						insideWord = false;
					}
					if ( firstNewLine && character == '\n'){
						if (count.GetValue() != 0){
						Console.WriteLine(count.GetValue().ToString());
						count.Reset();
						firstNewLine = false;
						}
					}
					else if (character == '\n')
						firstNewLine = true;
					else firstNewLine = false;
				}
				if (count.GetValue() != 0)
					Console.WriteLine(count.GetValue().ToString());

			}
		}
		
	}
	
	class WordCounter {
		private int _count;
		// private Dictionary<string,int> _wordFrequency; 
		// private LinkedList<string> keys;
		public WordCounter(){
			_count = 0;
			// _wordFrequency = new Dictionary<string,int>();
			// keys = new LinkedList<string>();
		}
		public void Add(){
			_count++;
		}
		public int GetValue(){
			return _count;
		}
		public void Reset(){
			_count = 0;
		}
		/*
		public void Add(string key){
			if (_wordFrequency.ContainsKey(key))
				_wordFrequency[key]++;
			else{
				_wordFrequency[key] = 1;
				keys.AddLast(key);
			}
		}
		
		public void GetFrequency(){
			var sortedKeys = keys.OrderBy(s => s);		
			foreach (string key in sortedKeys)
			{
			   Console.WriteLine("{0}: {1}",key,_wordFrequency[key]);
			}
		}
		*/
	}

	internal class Program
 	{
 		static void Main(string[] args)
 		{
			if (args.Length != 1){
				Console.WriteLine("Argument Error");
				return;
			}
			if (!File.Exists(args[0])){
				Console.WriteLine("File Error");
				return;
			}
			FileLoader file = new FileLoader(args[0]);
			/* Solution for Word Counting task
			Console.WriteLine(file.count.GetValue().ToString());
			*/
			// TODO: Solution for Word Frequency task
			/// file.count.GetFrequency();
			

		}
 	}
 }

