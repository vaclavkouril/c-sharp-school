namespace TokenProcessingFramework;

#nullable enable

public interface ITokenProcessor {
	public void ProcessToken(Token token);
	public void Finish();
}
