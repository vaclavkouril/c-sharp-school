using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable enable

namespace FileProcessingConsoleAppFramework {

	public class AppErrorHandler {
		protected TextWriter ErrorOutput { get; private init; }

		public AppErrorHandler(TextWriter errorOutput) {
			ErrorOutput = errorOutput;
		}

		public void ExecuteProgram(IProgramCore programCore, string[] args) {
			try {
				programCore.Run(args);
			} catch (InvalidArgumentsApplicationException) {
				ErrorOutput.WriteLine("Argument Error");
			} catch (FileAccessErrorApplicationException) {
				ErrorOutput.WriteLine("File Error");
			}
		}
	}

}