namespace TokenProcessingFramework;

#nullable enable

public enum TokenType { EndOfInput = 0, Word, EndOfLine, EndOfParagraph }

public readonly record struct Token(TokenType Type, string? Value = null) {
	public Token(string word) : this(TokenType.Word, word) { }
}
