using System;
using System.IO;

using FileProcessingConsoleAppFramework;

#nullable enable

namespace TableSummatorApp;

public class TableSummatorAppErrorHandler : AppErrorHandler {
	public TableSummatorAppErrorHandler(TextWriter errorOutput) : base(errorOutput) { }

	public new void ExecuteProgram(IProgramCore programCore, string[] args) {
		try {
			base.ExecuteProgram(programCore, args);
		} catch (InvalidFileFormatApplicationException) {
			ErrorOutput.WriteLine("Invalid File Format");
		} catch (InvalidIntegerValueApplicationException) {
			ErrorOutput.WriteLine("Invalid Integer Value");
		} catch (NonExistentColumnNameApplicationException) {
			ErrorOutput.WriteLine("Non-existent Column Name");
		}
	}
}
