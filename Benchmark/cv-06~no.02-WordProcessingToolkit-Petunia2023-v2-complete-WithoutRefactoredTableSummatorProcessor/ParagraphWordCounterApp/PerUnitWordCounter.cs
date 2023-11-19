using System;
using System.IO;

using TokenProcessingFramework;
using LegacyWordCounterApp;

namespace ParagraphWordCounterApp;

public record class PerUnitWordCounter(TextWriter Writer) : ITokenProcessor {
	private WordCounter? _wordCounter = null;

	public void ProcessToken(Token token) {
		if (token is { Type: TokenType.Word, Value: string word }) {
			if (_wordCounter is null) {
				_wordCounter = new WordCounter(Writer);
			}
			_wordCounter.ProcessWord(word);
		} else {
			Finish();
		}
	}

	public void Finish() {
		if (_wordCounter is not null) {
			// Writer.Write("Words in unit: ");
			_wordCounter.Finish();
			_wordCounter = null;
		}
	}
}