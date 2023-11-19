namespace TokenProcessingFramework;

public record class DebugPrinterTokenReaderDecorator(ITokenReader Reader) : ITokenReader {
	public Token ReadToken() {
		var token = Reader.ReadToken();
		Console.WriteLine(token);
		return token;
	}
}

public record class V1_2BUGS_ParagraphDetectingTokenReaderDecorator(ITokenReader Reader) : ITokenReader {
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

public record class V2_1BugFixed_1BugLeft_ParagraphDetectingTokenReaderDecorator(ITokenReader Reader) : ITokenReader {
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

			if (newLinesFound > 1 || token.Type == TokenType.EndOfInput) {
				_nextToken = token;
				return new Token(TokenType.EndOfParagraph);
			}
			return token;
		}
	}
}

public record class V3_CORRECT_ParagraphDetectingTokenReaderDecorator(ITokenReader Reader) : ITokenReader {
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