namespace LegacyWordCounterApp {

	// Readers are using a pull model
	public interface IWordReader {
		/// <summary>
		/// Returns next word from input, if end of input is not reached.
		/// If next word is not available yet, it blocks and waits for more words to become available or for end of input.
		/// </summary>
		/// <returns>Next word from input, null if end of input is reached.</returns>
		string? ReadWord();
	}

	// Processors are using a push model.
	public interface IWordProcessor {
		void ProcessWord(string word);
		void Finish();
	}

	public class Program {
		static void Main(string[] args) {
			Console.WriteLine("Hello, World!");
		}

		public static void ProcessWords(IWordReader reader, IWordProcessor processor) {
			string? word;

			while ((word = reader.ReadWord()) != null) {
				processor.ProcessWord(word);
			}
			processor.Finish();
		}
	}
}