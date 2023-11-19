using FileProcessingConsoleAppFramework;
using System;
using TokenProcessingFramework;

#nullable enable

namespace TableSummatorApp;

internal class Program {
	static void Main(string[] args) {
		var appErrorHandler = new TableSummatorAppErrorHandler(Console.Out);
		appErrorHandler.ExecuteProgram(new TableSummatorProgramCore(), args);
	}
}

public class TableSummatorProgramCore : IProgramCore {
	public void Run(string[] args) {
		var state = new ArgsToInputOutputState(args);
		try {
			state.CheckArgumentCount(3);
			state.OpenInputFile(0);
			state.OpenOutputFile(1);

			var tokenReader = new ByLinesTokenReader(state.InputReader!);
			var processor = new TableSummatorProcessor(state.OutputWriter!, args[2]);

			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);
		} finally {
			state.Dispose();
		}
	}
}
