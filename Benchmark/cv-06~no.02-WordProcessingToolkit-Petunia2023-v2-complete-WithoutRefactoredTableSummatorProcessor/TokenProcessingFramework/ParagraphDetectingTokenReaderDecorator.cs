using System;

namespace TokenProcessingFramework;

#nullable enable

//
// Version 1: contains 2 bugs:
//

/*
public record class ParagraphDetectingTokenReaderDecorator(ITokenReader Reader) : ITokenReader {
	private Token? _nextToken = null;

	public Token ReadToken() {
		if (_nextToken is not null) {
			var token = _nextToken.Value;
			_nextToken = null;
			return token;
		} else {
			int newLinesFound = 0;

			Token token;
			while ((token = Reader.ReadToken()).Type == TokenType.EndOfLine) {
				newLinesFound++;
			}

			if (newLinesFound > 1) {
				_nextToken = token;
				return new Token(TokenType.EndOfParagraph);
			}
			return token;
		}
	}
}
*/

//
// Version 3: CORRECT!
//


public record class ParagraphDetectingTokenReaderDecorator(ITokenReader Reader) : ITokenReader {
	private bool _firstParagraphStarted = false;
	private Token? _nextToken = null;

	public Token ReadToken() {
		if (_nextToken is not null) {
			var token = _nextToken.Value;
			_nextToken = null;
			return token;
		} else {
			int newLinesFound = 0;

			Token token;
			while ((token = Reader.ReadToken()).Type == TokenType.EndOfLine) {
				newLinesFound++;
			}

			if ((newLinesFound > 1 && _firstParagraphStarted) || token.Type == TokenType.EndOfInput) {
				_nextToken = token;
				return new Token(TokenType.EndOfParagraph);
			} else {
				_firstParagraphStarted = true;
				return token;
			}
		}
	}
}
