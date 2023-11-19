using TokenProcessingFramework;

namespace TokenProcessingFrameworkSimplified_UnitTests {
	public class ParagraphDetectingTokenReaderDecoratorTests {
		[Fact]
		public void ReadToken_ShouldReturnEndOfInput_WhenInputReaderReturnsEndOfInput() {
			// Arrange
			var inputReader = new FakeTokenReader(new Token(TokenType.EndOfInput));
			var decorator = new ParagraphDetectingTokenReaderDecorator(inputReader);

			// Act
			var token = decorator.ReadToken();

			// Assert
			Assert.Equal(TokenType.EndOfInput, token.Type);
		}

		[Fact]
		public void ReadToken_ShouldReturnEndOfLine_WhenInputReaderReturnsEndOfLine() {
			// Arrange
			var inputReader = new FakeTokenReader(new Token(TokenType.EndOfLine));
			var decorator = new ParagraphDetectingTokenReaderDecorator(inputReader);

			// Act
			var token = decorator.ReadToken();

			// Assert
			Assert.Equal(TokenType.EndOfLine, token.Type);
		}

		[Fact]
		public void ReadToken_ShouldReturnEndOfParagraph_WhenInputReaderReturnsMultipleEndOfLines() {
			// Arrange
			var inputReader = new FakeTokenReader(
				new Token(TokenType.EndOfLine),
				new Token(TokenType.EndOfLine),
				new Token(TokenType.Word, "Hello")
			);
			var decorator = new ParagraphDetectingTokenReaderDecorator(inputReader);

			// Act
			var token1 = decorator.ReadToken(); // Skips first EndOfLine
			var token2 = decorator.ReadToken(); // Detects second EndOfLine and returns EndOfParagraph
			var token3 = decorator.ReadToken(); // Returns the next word token

			// Assert
			Assert.Equal(TokenType.Word, token1.Type);
			Assert.Equal(TokenType.EndOfParagraph, token2.Type);
			Assert.Equal(TokenType.Word, token3.Type);
		}

		[Fact]
		public void ReadToken_ShouldReturnWord_WhenInputReaderReturnsWord() {
			// Arrange
			var inputReader = new FakeTokenReader(new Token(TokenType.Word, "Hello"));
			var decorator = new ParagraphDetectingTokenReaderDecorator(inputReader);

			// Act
			var token = decorator.ReadToken();

			// Assert
			Assert.Equal(TokenType.Word, token.Type);
			Assert.Equal("Hello", token.Value);
		}

		private class FakeTokenReader : ITokenReader {
			private readonly Queue<Token> _tokens;

			public FakeTokenReader(params Token[] tokens) {
				_tokens = new Queue<Token>(tokens);
			}

			public Token ReadToken() {
				if (_tokens.TryDequeue(out var token)) {
					return token;
				}
				return new Token(TokenType.EndOfInput);
			}
		}
	}
}