namespace WordToTokenAdapters;

using WordProcessingFramework;
using TokenProcessingFramework;

public record class TokenProcessorFromWordProcessorAdapter(IWordProcessor WordProcessor) : ITokenProcessor {
	public void ProcessToken(Token token) {
		if (token.Type == TokenType.Word) {
			WordProcessor.ProcessWord(token.Value!);
		} else {
			// Skip all other tokens
		}
	}

	public void Finish() {
		WordProcessor.Finish();
	}
}
