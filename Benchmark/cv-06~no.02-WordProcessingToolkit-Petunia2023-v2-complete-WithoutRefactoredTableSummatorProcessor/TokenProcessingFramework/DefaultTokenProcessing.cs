using System;

namespace TokenProcessingFramework;

#nullable enable

public static class DefaultTokenProcessing {
	public static void ProcessTokensUntilEndOfInput(ITokenReader reader, ITokenProcessor processor) {
		while (reader.ReadToken() is { Type: not TokenType.EndOfInput } token) {
			processor.ProcessToken(token);
		}

		processor.Finish();
	}
}