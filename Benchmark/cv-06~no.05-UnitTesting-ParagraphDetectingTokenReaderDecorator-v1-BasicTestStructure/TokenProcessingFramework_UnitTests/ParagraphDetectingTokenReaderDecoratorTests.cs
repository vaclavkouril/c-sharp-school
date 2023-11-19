namespace TokenProcessingFramework_UnitTests {
	public class FakeTokenReader : ITokenReader {
		private readonly Queue<Token> _tokens;

		public FakeTokenReader(IEnumerable<Token> tokens) {
			this._tokens = new Queue<Token>(tokens);
		}

		public Token ReadToken() {
			if (_tokens.Count == 0) {
				throw new InvalidOperationException("No more tokens to read.");
			}

			return _tokens.Dequeue();
		}
	}

	public class ParagraphDetectingTokenReaderDecoratorTests {
		private static List<Token> ReadAllTokens(ITokenReader tokenReader) {
			var tokens = new List<Token>();

			Token token;
			do {
				token = tokenReader.ReadToken();
				tokens.Add(token);
			} while (token.Type != TokenType.EndOfInput);

			return tokens;
		}

		[Fact]
		public void ReadToken_OneParagraph_NoExtraNewLines() {
			// Arrange
			var tokenReader = new FakeTokenReader(new[] {
				new Token("The"), new Token("rain"), new Token("in"),
				new Token(TokenType.EndOfInput)
			});
			var expectedTokens = new[] {
				new Token("The"), new Token("rain"), new Token("in"),
				new Token(TokenType.EndOfParagraph), new Token(TokenType.EndOfInput)
			};

			var decorator = new ParagraphDetectingTokenReaderDecorator(tokenReader);

			// Act
			var tokens = ReadAllTokens(decorator);

			// Assert
			Assert.Equal(expectedTokens, tokens);
		}

		[Fact]
		public void ReadToken_OneParagraph_NewLineInsideParagraph() {
			// Arrange
			var tokenReader = new FakeTokenReader(new[] {
				new Token("The"), new Token("rain"), new Token("in"),
				new Token(TokenType.EndOfLine),
				new Token("Spain"), new Token("falls"),
				new Token(TokenType.EndOfInput)
			});
			var expectedTokens = new[] {
				new Token("The"), new Token("rain"), new Token("in"),
				new Token("Spain"), new Token("falls"),
				new Token(TokenType.EndOfParagraph), new Token(TokenType.EndOfInput)
			};

			var decorator = new ParagraphDetectingTokenReaderDecorator(tokenReader);

			// Act
			var tokens = ReadAllTokens(decorator);

			// Assert
			Assert.Equal(expectedTokens, tokens);
		}

		[Fact]
		public void ReadToken_OneParagraph_TwoNewLinesBeforeEndOfInput() {
			// Arrange
			var tokenReader = new FakeTokenReader(new[] { 
				new Token("The"), new Token("rain"), new Token("in"),
				new Token(TokenType.EndOfLine), new Token(TokenType.EndOfLine), new Token(TokenType.EndOfInput)
			});
			var expectedTokens = new[] {
				new Token("The"), new Token("rain"), new Token("in"),
				new Token(TokenType.EndOfParagraph), new Token(TokenType.EndOfInput)
			};

			var decorator = new ParagraphDetectingTokenReaderDecorator(tokenReader);

			// Act
			var tokens = ReadAllTokens(decorator);

			// Assert
			Assert.Equal(expectedTokens, tokens);
		}
	}
}