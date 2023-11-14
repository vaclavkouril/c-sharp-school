using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using WordProcessingFramework;

#nullable enable

namespace LegacyWordCounterApp {

	public class WordCounter : IWordProcessor {
		private readonly TextWriter _writer;
		private int _wordCount = 0;

		public WordCounter(TextWriter writer) {
			this._writer = writer;
		}

		public void ProcessWord(string word) {
			_wordCount++;
		}

		public void Finish() {
			_writer.WriteLine(_wordCount);
		}
	}

}