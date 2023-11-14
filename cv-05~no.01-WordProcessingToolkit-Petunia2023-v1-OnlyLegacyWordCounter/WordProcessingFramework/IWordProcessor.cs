namespace WordProcessingFramework {

	// Processors are using a push model.
	public interface IWordProcessor {
		void ProcessWord(string word);
		void Finish();
	}

}