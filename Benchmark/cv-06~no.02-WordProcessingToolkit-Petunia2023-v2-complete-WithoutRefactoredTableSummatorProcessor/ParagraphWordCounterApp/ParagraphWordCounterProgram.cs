using System;
using System.IO;

using FileProcessingConsoleAppFramework;
using TokenProcessingFramework;

#nullable enable

namespace ParagraphWordCounterApp;

internal class Program {
	static void Main(string[] args) {
		var appErrorHandler = new AppErrorHandler(Console.Out);
		appErrorHandler.ExecuteProgram(new ParagraphWordCounterProgramCore(), args);
	}
}

public class ParagraphWordCounterProgramCore : IProgramCore {
	public void Run(string[] args) {
		var state = new ArgsToInputOutputState(args);
		try {
			state.CheckArgumentCount(1);
			state.OpenInputFile(0);

			var tokenReader = new ParagraphDetectingTokenReaderDecorator(
				new ByLinesTokenReader(state.InputReader!)
			);
			var processor = new PerUnitWordCounter(Console.Out);

			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);
		} finally {
			state.Dispose();
		}
	}
}
