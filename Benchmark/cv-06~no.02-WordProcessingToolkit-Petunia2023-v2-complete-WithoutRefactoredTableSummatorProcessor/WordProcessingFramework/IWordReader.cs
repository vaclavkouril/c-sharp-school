
#nullable enable

namespace WordProcessingFramework {

	// Readers are using a pull model
	public interface IWordReader {
		/// <summary>
		/// Returns next word from input, and wait for more words become available until end of input is reached.
		/// </summary>
		/// <returns>Next word from input, null if end of input is reached.</returns>
		string? ReadWord();
	}

}