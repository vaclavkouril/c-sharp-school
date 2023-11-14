using System;
using System.Collections.Generic;
using System.IO;

namespace TokenProcessingFramework;

#nullable enable

// by ChatGPT: query 1 (too generic query)
public class ChatGPTQueryOne_ByLinesTokenReader : ITokenReader, IDisposable {
	private static readonly char[] Whitespaces = new[] { ' ', '\t' };

	private readonly TextReader _reader;
	private string[] _words = Array.Empty<string>();
	private int _nextWord = 0;

	public ChatGPTQueryOne_ByLinesTokenReader(TextReader reader) {
		_reader = reader;
	}

	public Token ReadToken() {
		while (_nextWord >= _words.Length) {
			if (_reader.ReadLine() is string line) {
				_words = line.Split(Whitespaces, StringSplitOptions.RemoveEmptyEntries);
				_nextWord = 0;
				return new Token(TokenType.EndOfLine);
			} else {
				return new Token(TokenType.EndOfInput);
			}
		}

		return new Token(TokenType.Word, _words[_nextWord++]);
	}

	public void Dispose() {
		_reader.Dispose();
	}
}

// by ChatGPT, but patched to be correct (added _endOfLineReported field):
public class ByLinesTokenReader : ITokenReader, IDisposable {
	private static readonly char[] Whitespaces = new[] { ' ', '\t' };

	private readonly TextReader _reader;
	private string[] _words = Array.Empty<string>();
	private int _nextWord = 0;
	private bool _endOfLineReported = true;

	public ByLinesTokenReader(TextReader reader) {
		_reader = reader;
	}

	public Token ReadToken() {
		while (_nextWord >= _words.Length) {
			if (!_endOfLineReported) {
				_endOfLineReported = true;
				return new Token(TokenType.EndOfLine);
			}

			if (_reader.ReadLine() is string line) {
				_words = line.Split(Whitespaces, StringSplitOptions.RemoveEmptyEntries);
				_nextWord = 0;
				_endOfLineReported = false;
			} else {
				return new Token(TokenType.EndOfInput);
			}
		}

		return new Token(TokenType.Word, _words[_nextWord++]);
	}

	public void Dispose() {
		_reader.Dispose();
	}
}

// by ChatGPT: query 2 (more specific query)
public class ChatGPTQueryTwo_ByLinesTokenReader : ITokenReader, IDisposable {
	private static readonly char[] Whitespaces = new[] { ' ', '\t' };

	private readonly TextReader _reader;
	private List<Token> _tokens = new List<Token>();
	private int _nextToken = 0;

	public ChatGPTQueryTwo_ByLinesTokenReader(TextReader reader) {
		_reader = reader;
		ReadLineTokens();
	}

	public Token ReadToken() {
		if (_nextToken >= _tokens.Count) {
			// No more tokens available
			return new Token(TokenType.EndOfInput);
		}

		Token token = _tokens[_nextToken++];
		if (_nextToken >= _tokens.Count) {
			// If this was the last token, read more tokens from the next line
			ReadLineTokens();
		}

		return token;
	}

	private void ReadLineTokens() {
		_tokens.Clear();
		if (_reader.ReadLine() is string line) {
			string[] words = line.Split(Whitespaces, StringSplitOptions.RemoveEmptyEntries);
			foreach (string word in words) {
				_tokens.Add(new Token(TokenType.Word, word));
			}
			_tokens.Add(new Token(TokenType.EndOfLine));
		} else {
			// End of input
			_tokens.Add(new Token(TokenType.EndOfInput));
		}
		_nextToken = 0;
	}

	public void Dispose() {
		_reader.Dispose();
	}
}