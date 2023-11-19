using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FileProcessingConsoleAppFramework;
using TokenProcessingFramework;
using WordToTokenAdapters;

#nullable enable

namespace WordCounterApp {

	internal class Program {
		static void Main(string[] args) {
			var appErrorHandler = new AppErrorHandler(Console.Out);
			appErrorHandler.ExecuteProgram(new WordCounterProgramCore(), args);
		}
	}

	public class WordCounterProgramCore : IProgramCore {
		public void Run(string[] args) {
			var state = new ArgsToInputOutputState(args);
			try {
				state.CheckArgumentCount(1);
				state.OpenInputFile(0);

				var tokenReader = new ByLinesTokenReader(state.InputReader!);
				var processor =
					new TokenProcessorFromWordProcessorAdapter(
						new LegacyWordCounterApp.WordCounter(Console.Out)
					);

				DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);
			} finally {
				state.Dispose();
			}
		}
	}

}

