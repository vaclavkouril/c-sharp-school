namespace TokenProcessingFramework_UnitTests {

	using ParagraphDetectingTokenReaderDecorator = V1_2BUGS_ParagraphDetectingTokenReaderDecorator;
	// using ParagraphDetectingTokenReaderDecorator = V2_1BugFixed_1BugLeft_ParagraphDetectingTokenReaderDecorator;
	// using ParagraphDetectingTokenReaderDecorator = V3_CORRECT_ParagraphDetectingTokenReaderDecorator;

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

		private static readonly Token NEWLINE = new Token(TokenType.EndOfLine);		// Do not rename to EOL - can be easily confused with EOI
		private static readonly Token PARA = new Token(TokenType.EndOfParagraph);
		private static readonly Token EOI = new Token(TokenType.EndOfInput);

		private static Token WORD(string word) => new Token(word);

		[Fact]
		public void ReadToken_OneParagraph_NoExtraNewLines() {
			// Arrange
			var tokenReader = new FakeTokenReader(new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				EOI
			});
			var expectedTokens = new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				PARA, EOI
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
				WORD("The"), WORD("rain"), WORD("in"),
				NEWLINE,
				WORD("Spain"), WORD("falls"),
				EOI 
			});
			var expectedTokens = new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				WORD("Spain"), WORD("falls"),
				PARA, EOI
			};

			var decorator = new ParagraphDetectingTokenReaderDecorator(tokenReader);

			// Act
			var tokens = ReadAllTokens(decorator);

			// Assert
			Assert.Equal(expectedTokens, tokens);
		}

		[Fact]
		public void ReadToken_OneParagraph_TwoNewLinesInsideParagraph() {
			// Arrange
			var tokenReader = new FakeTokenReader(new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				NEWLINE,
				WORD("Spain"), WORD("falls"),
				NEWLINE,
				WORD("mainly"), WORD("on"), WORD("the"), WORD("plain."),
				EOI
			});
			var expectedTokens = new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				WORD("Spain"), WORD("falls"),
				WORD("mainly"), WORD("on"), WORD("the"), WORD("plain."),
				PARA, EOI
			};

			var decorator = new ParagraphDetectingTokenReaderDecorator(tokenReader);

			// Act
			var tokens = ReadAllTokens(decorator);

			// Assert
			Assert.Equal(expectedTokens, tokens);
		}

		[Fact]
		public void ReadToken_OneParagraph_OneWordTotal() {
			// Arrange
			var tokenReader = new FakeTokenReader(new[] {
				WORD("The"),
				EOI
			});
			var expectedTokens = new[] {
				WORD("The"),
				PARA, EOI
			};

			var decorator = new ParagraphDetectingTokenReaderDecorator(tokenReader);

			// Act
			var tokens = ReadAllTokens(decorator);

			// Assert
			Assert.Equal(expectedTokens, tokens);
		}

		[Fact]
		public void ReadToken_OneParagraph_OneWordPerLine() {
			// Arrange
			var tokenReader = new FakeTokenReader(new[] {
				WORD("The"), NEWLINE,
				WORD("rain"), NEWLINE,
				WORD("in"), NEWLINE,
				EOI
			});
			var expectedTokens = new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				PARA, EOI
			};

			var decorator = new ParagraphDetectingTokenReaderDecorator(tokenReader);

			// Act
			var tokens = ReadAllTokens(decorator);

			// Assert
			Assert.Equal(expectedTokens, tokens);
		}

		[Fact]
		public void ReadToken_TwoParagraphs_NoExtraNewLines() {
			// Arrange
			var tokenReader = new FakeTokenReader(new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				NEWLINE, NEWLINE,
				WORD("Spain"), WORD("falls"),
				EOI
			});
			var expectedTokens = new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				PARA,
				WORD("Spain"), WORD("falls"),
				PARA, EOI
			};

			var decorator = new ParagraphDetectingTokenReaderDecorator(tokenReader);

			// Act
			var tokens = ReadAllTokens(decorator);

			// Assert
			Assert.Equal(expectedTokens, tokens);
		}

		[Fact]
		public void ReadToken_OneParagraph_StartsWithOneNewLine() {
			// Arrange
			var tokenReader = new FakeTokenReader(new[] {
				NEWLINE,
				WORD("The"), WORD("rain"), WORD("in"),
				EOI
			});
			var expectedTokens = new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				PARA, EOI
			};

			var decorator = new ParagraphDetectingTokenReaderDecorator(tokenReader);

			// Act
			var tokens = ReadAllTokens(decorator);

			// Assert
			Assert.Equal(expectedTokens, tokens);
		}

		[Fact]
		public void ReadToken_OneParagraph_StartsWithTwoNewLines() {
			// Arrange
			var tokenReader = new FakeTokenReader(new[] {
				NEWLINE, NEWLINE,
				WORD("The"), WORD("rain"), WORD("in"),
				EOI
			});
			var expectedTokens = new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				PARA, EOI
			};

			var decorator = new ParagraphDetectingTokenReaderDecorator(tokenReader);

			// Act
			var tokens = ReadAllTokens(decorator);

			// Assert
			Assert.Equal(expectedTokens, tokens);
		}

		[Fact]
		public void ReadToken_OneParagraph_StartsWithThreeNewLines() {
			// Arrange
			var tokenReader = new FakeTokenReader(new[] {
				NEWLINE, NEWLINE, NEWLINE,
				WORD("The"), WORD("rain"), WORD("in"),
				EOI
			});
			var expectedTokens = new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				PARA, EOI
			};

			var decorator = new ParagraphDetectingTokenReaderDecorator(tokenReader);

			// Act
			var tokens = ReadAllTokens(decorator);

			// Assert
			Assert.Equal(expectedTokens, tokens);
		}

		[Fact]
		public void ReadToken_OneParagraph_OneNewLinesBeforeEndOfInput() {
			// Arrange
			var tokenReader = new FakeTokenReader(new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				NEWLINE, EOI
			});
			var expectedTokens = new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				PARA, EOI
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
				WORD("The"), WORD("rain"), WORD("in"),
				NEWLINE, NEWLINE, EOI
			});
			var expectedTokens = new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				PARA, EOI
			};

			var decorator = new ParagraphDetectingTokenReaderDecorator(tokenReader);

			// Act
			var tokens = ReadAllTokens(decorator);

			// Assert
			Assert.Equal(expectedTokens, tokens);
		}

		[Fact]
		public void ReadToken_OneParagraph_ThreeNewLinesBeforeEndOfInput() {
			// Arrange
			var tokenReader = new FakeTokenReader(new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				NEWLINE, NEWLINE, NEWLINE, EOI
			});
			var expectedTokens = new[] {
				WORD("The"), WORD("rain"), WORD("in"),
				PARA, EOI
			};

			var decorator = new ParagraphDetectingTokenReaderDecorator(tokenReader);

			// Act
			var tokens = ReadAllTokens(decorator);

			// Assert
			Assert.Equal(expectedTokens, tokens);
		}

		// FIXME: More tests needed - not all scenarios covered yet !!!
	}
}