using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace TextJusti
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var state = new ProgramInputOutputState();
			if (!state.InitializeFromCommandLineArgs(args)) {
				return;
			}
	    TextFormater format = new TextFormater(state.Reader, state.Writer, state.Number);
	    format.Execute();
	    state.Dispose();
	}
    }	
    public class ProgramInputOutputState : IDisposable
	{
		public const string ArgumentErrorMessage = "Argument Error";
		public const string FileErrorMessage = "File Error";
		public TextReader? Reader { get; private set; }
		public TextWriter? Writer { get; private set; }
		public int Number { get; private set; }

		public bool InitializeFromCommandLineArgs(string[] args) {
			if (args.Length != 3) {
				Console.WriteLine(ArgumentErrorMessage);
				return false;
			}

			try {
				int n = Convert.ToInt32(args[2]);
				if ( n > 0)
					Number = n;
				else{
					Console.WriteLine(ArgumentErrorMessage);
					return false;
				}

			}
			catch{
				Console.WriteLine(ArgumentErrorMessage);	

			}	
			
			try {
				Reader = new StreamReader(args[0]);
			} catch (IOException) {
				Console.WriteLine(FileErrorMessage);
				return false;
			} catch (UnauthorizedAccessException) {
				Console.WriteLine(FileErrorMessage);
				return false;
			} catch (ArgumentException) {
				Console.WriteLine(FileErrorMessage);
				return false;
			}

			try {
				Writer = new StreamWriter(args[1]);
			} catch (IOException) {
				Console.WriteLine(FileErrorMessage);
				return false;
			} catch (UnauthorizedAccessException) {
				Console.WriteLine(FileErrorMessage);
				return false;
			} catch (ArgumentException) {
				Console.WriteLine(FileErrorMessage);
				return false;
			}
		return true;
	}
	public void Dispose() 
	{
		Reader?.Dispose();
		Writer?.Dispose();
	}
    }
    public class TextFormater
    {
	private TextReader _reader;
	private TextWriter _writer;
	private int _maxLineChars;

	private bool firstNewLine = false;
	private bool secondNewLine = false;
	
	LinkedList<string> line = new LinkedList<string>();
	private int lineCharCount = 0;

	public TextFormater(TextReader reader, TextWriter writer, int NofCharsOnLine) {
		_reader = reader;
		_writer = writer;
		_maxLineChars = NofCharsOnLine;
	}

	public void Execute(){
	    bool firstRun = true;

	    string word = "";
            bool insideWord = false;

	    int charInt;
	    while ((charInt = _reader.Read())!= -1){
	        char character = (char)charInt;
		if(!char.IsWhiteSpace(character)){
			if (secondNewLine && !firstRun)
				writeParagraph();
			resetNewLineDetector();
			firstRun = false;
			insideWord=true;
			word+=character;
		}
		else if(char.IsWhiteSpace(character) && insideWord){
			if (_maxLineChars < lineCharCount + line.Count + word.Length){
					if (lineCharCount != 0)
						WriteLine();
				}
			lineCharCount += word.Length;
			line.AddLast(word);
			word = "";
			insideWord = false;
		}
		if ( character == '\n')
			newLineDetected();
	    }
	    if (word.Length != 0){
		line.AddLast(word);
		lineCharCount += word.Length;
	    }
	    writeLast();
	}
	private void WriteLine(){
	    /*
	    if (line.Count == 1){
		_writer.WriteLine(line.First.Value);
		line.Clear();
		lineCharCount = 0;
		return;
	    }*/

	    int spacesToAdd = _maxLineChars - lineCharCount;
            int spacesCount = line.Count - 1;
            int extraSpaces;
            int spacesPerGap;

	    if (line.Count == 1){
		extraSpaces = 0;
		spacesPerGap = 0;
	    }
	    else{
		extraSpaces = spacesToAdd % spacesCount;
		spacesPerGap = spacesToAdd / spacesCount;


	    }
            var formattedLine = new List<string>();

            foreach (var word in line)
            {
                formattedLine.Add(word);

                if (line.Count > 1 && spacesCount > 0)
                {
                    for (int i = 0; i < spacesPerGap; i++)
                    {
                        formattedLine.Add(" ");
                    }

                    if (extraSpaces > 0)
                    {
                        formattedLine.Add(" ");
                        extraSpaces--;
                    }
		    spacesCount--;
                }
            }
	    _writer.WriteLine(string.Join("", formattedLine));

            line.Clear();
            lineCharCount = 0;
        }
	private void newLineDetected(){
		if (firstNewLine){
			secondNewLine = true;
		}
		else firstNewLine = true;
	}
	private void writeParagraph(){
		if (line.Count > 0)
			_writer.WriteLine(string.Join(" ",line));
		line.Clear();
		lineCharCount = 0;

		_writer.WriteLine();
	}
	private void writeLast(){
		if (line.Count > 0)
			_writer.WriteLine(string.Join(" ",line));
		line.Clear();
		lineCharCount = 0;
	}

	private void resetNewLineDetector(){
		firstNewLine = false;
		secondNewLine =  false;

	}
    }
}
