using System;

namespace TokenProcessingFramework;

#nullable enable

public record class DebugPrinterTokenReaderDecorator(ITokenReader Reader) : ITokenReader {
	public Token ReadToken() {
		var token = Reader.ReadToken();
		Console.WriteLine(token);
		return token;
	}
}