using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WordProcessingFramework;

#nullable enable

namespace LegacyWordCounterApp {
	internal class Program {
		static void Main(string[] args) {
			if (args.Length < 1 || args[0] == "") {
				ReportArgumentError();
				return;
			}

			IWordReader? wordReader = null;
			try {
				// Input
				var textReader = new StreamReader(args[0]);
				wordReader = new ByLinesWordReader(textReader);

				// Processor and output
				var processor = new WordCounter(Console.Out);

				// Actual execution
				ProcessAllWords(wordReader, processor);

			} catch (FileNotFoundException) {
				ReportFileError();
			} catch (IOException) {
				ReportFileError();
			} catch (UnauthorizedAccessException) {
				ReportFileError();
			} finally {
				if (wordReader is IDisposable disposable) disposable.Dispose();
			}
		}

		public static void ReportFileError() {
			Console.WriteLine("File Error");
		}

		public static void ReportArgumentError() {
			Console.WriteLine("Argument Error");
		}

		// Default word processing algorithm
		public static void ProcessAllWords(IWordReader reader, IWordProcessor processor) {
			while (reader.ReadWord() is string word) {
				processor.ProcessWord(word);
			}

			processor.Finish();
		}
	}
}