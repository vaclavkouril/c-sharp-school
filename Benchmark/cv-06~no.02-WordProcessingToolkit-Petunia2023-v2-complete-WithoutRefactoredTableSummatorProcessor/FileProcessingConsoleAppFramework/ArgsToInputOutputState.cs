using System;
using System.IO;

#nullable enable

namespace FileProcessingConsoleAppFramework {

	public class ArgsToInputOutputState : IDisposable {
		private readonly string[] _args;

		public ArgsToInputOutputState(string[] args) {
			_args = args;
		}

		public TextReader? InputReader { get; private set; } = null;
		public TextWriter? OutputWriter { get; private set; } = null;

		public bool CheckArgumentCount(int expectedArgumentCount) {
			if (_args.Length == expectedArgumentCount) {
				return true;
			} else {
				throw new InvalidArgumentsApplicationException();
			}
		}

		public void OpenInputFile(int fileNameArgIndex) {
			try {
				InputReader = new StreamReader(_args[fileNameArgIndex]);
			} catch (IOException) {
				throw new FileAccessErrorApplicationException();
			} catch (UnauthorizedAccessException) {
				throw new FileAccessErrorApplicationException();
			} catch (ArgumentException) {
				throw new FileAccessErrorApplicationException();
			}
		}

		public void OpenOutputFile(int fileNameArgIndex) {
			try {
				OutputWriter = new StreamWriter(_args[fileNameArgIndex]);
			} catch (IOException) {
				throw new FileAccessErrorApplicationException();
			} catch (UnauthorizedAccessException) {
				throw new FileAccessErrorApplicationException();
			} catch (ArgumentException) {
				throw new FileAccessErrorApplicationException();
			}
		}

		public void Dispose() {
			InputReader?.Dispose();
			OutputWriter?.Dispose();
		}
	}

}