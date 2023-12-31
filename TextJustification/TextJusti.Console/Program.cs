﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace TextJusti
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string delimiter = " ";
	    string endOfLine = "";
	    List<string> files = new List<string>();
	    var state = new ProgramInputOutputState();
	    if (!state.InitializeFromCommandLineArgs(args))return;
	    if (state.HighlightSpaces(args[0])){ 
		    delimiter = ".";
		    endOfLine = "<-";
	    }
	    else files.Add(args[0]);
	    
	    for (int i = 1; i < args.Length-2; i++) files.Add(args[i]);
	    TextFormater format = new TextFormater(files.ToArray(), state.Writer, state.Number, delimiter, endOfLine);
	    format.ExecuteMultiFiles();
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
			if (args.Length < 3){
				Console.WriteLine(ArgumentErrorMessage);
				return false;	
			}
			try {
				int n = Convert.ToInt32(args[args.Length-1]);
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
				Writer = new StreamWriter(args[args.Length-2]);
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
	/*		
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
*/

		return true;
	}
	public void Dispose() 
	{
		Reader?.Dispose();
		Writer?.Dispose();
	}
	public bool HighlightSpaces(string input_flag){
		if (input_flag == "--highlight-spaces") return true;
		else return false;
	}
    }
    public class TextFormater
    {
	private TextReader _reader;
	private string[]? _files;
	private TextWriter _writer;
	private int _maxLineChars;
	private string _delimiter;
	private string _endOfLine;

	private bool firstNewLine = false;
	private bool secondNewLine = false;
	
	LinkedList<string> line = new LinkedList<string>();
	private int lineCharCount = 0;

	public TextFormater(TextReader reader, TextWriter writer, int NofCharsOnLine) {
		_reader = reader;
		_writer = writer;
		_maxLineChars = NofCharsOnLine;
	}
	
	public TextFormater(string[] files, TextWriter writer, int NofCharsOnLine, string delimiter, string endOfLine = "") {
		_files = files;
		_writer = writer;
		_maxLineChars = NofCharsOnLine;
		_delimiter = delimiter;
		_endOfLine = endOfLine;
	}
	
	public void ExecuteMultiFiles(){
	    bool firstRun = true;

	    string word = "";
            bool insideWord = false;
	    StreamReader reader;  
	    int charInt;
	    for(int i = 0; i < _files.Length; i++ ){
		try{
			reader = new StreamReader(_files[i]);
		}
		catch{ continue;}
	    while ((charInt = reader.Read())!= -1){
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
	    }
	    writeLast();
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
                        formattedLine.Add(_delimiter);
                    }

                    if (extraSpaces > 0)
                    {
                        formattedLine.Add(_delimiter);
                        extraSpaces--;
                    }
		    spacesCount--;
                }
            }
	    _writer.WriteLine(string.Join("", formattedLine)+_endOfLine);

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
			_writer.WriteLine(string.Join(_delimiter,line)+_endOfLine);
		line.Clear();
		lineCharCount = 0;

		_writer.WriteLine(_endOfLine);
	}
	private void writeLast(){
		if (line.Count > 0)
			_writer.WriteLine(string.Join(_delimiter,line)+_endOfLine);
		line.Clear();
		lineCharCount = 0;
	}

	private void resetNewLineDetector(){
		firstNewLine = false;
		secondNewLine =  false;

	}
    }
}
