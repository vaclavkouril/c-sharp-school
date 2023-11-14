using System.IO;
using System;

#nullable enable

namespace WordProcessingFramework;

public class ByLinesWordReader : IWordReader, IDisposable {
	private static readonly char[] Whitespaces = new[] { ' ', '\t' };

	private readonly TextReader _reader;
	private string[] _words = Array.Empty<string>();
	private int _nextWord = 0;

	public ByLinesWordReader(TextReader reader) {
		_reader = reader;
	}

	public string? ReadWord() {
		while (_nextWord >= _words.Length) {
			if (_reader.ReadLine() is string line) {
				_words = line.Split(Whitespaces, StringSplitOptions.RemoveEmptyEntries);
				_nextWord = 0;
			} else {
				return null;
			}
		}

		return _words[_nextWord++];
	}

	public void Dispose() {
		_reader.Dispose();
	}
}