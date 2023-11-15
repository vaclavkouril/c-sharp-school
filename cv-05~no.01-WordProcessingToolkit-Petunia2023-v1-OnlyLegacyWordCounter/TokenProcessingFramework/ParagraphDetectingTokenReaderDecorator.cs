using System;

namespace TokenProcessingFramework;

#nullable enable

public record class ParagraphDetectingTokenReaderDecorator(ITokenReader Reader) : ITokenReader {
	private Token? _nextToken = null;
	private bool _firstWordFound = false;

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

			if (token.Type == TokenType.EndOfInput)
				return token;

			if (newLinesFound > 1) {
				if (!_firstWordFound){
					_firstWordFound = true;
					return token;
				}
				_nextToken = token;
				return new Token(TokenType.EndOfParagraph);
			}
			_firstWordFound = true;
			return token;
		}
	}
}
