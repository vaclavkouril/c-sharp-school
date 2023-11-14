using LegacyWordCounterApp;

namespace LegacyWordCounterApp_UnitTests {
	public class ProgramTests {

		class FakeWordReader : IWordReader {
			private string[] words;
			private int nextWord = 0;

			public FakeWordReader(string[] words) {
				this.words = words;
			}

			public string? ReadWord() {
				if (nextWord >= words.Length) {
					return null;
				}

				return words[nextWord++];
			}
		}

		class MockWordProcessor : IWordProcessor {
			public List<string> Words { get; } = new List<string>();
			public int FinishCallCounter { get; private set; } = 0;

			public void ProcessWord(string word) {
				Words.Add(word);
			}

			public void Finish() {
				FinishCallCounter++;
			}
		}

		[Fact]
		public void ProcessAllWords_NonEmptyListOfWords() {
			// Arrange
			string[] words = new[] { "The", "rain", "in", "Spain", "falls", "mainly", "on", "the", "plain." };

			var reader = new FakeWordReader(words);
			var processor = new MockWordProcessor();

			// Act
			Program.ProcessWords(reader, processor);

			// Assert
			Assert.Equal(words, processor.Words);
			Assert.Equal(1, processor.FinishCallCounter);
		}

		[Fact]
		public void ProcessAllWords_EmptyListOfWords() {
			// Arrange
			string[] words = new string[0];

			var reader = new FakeWordReader(words);
			var processor = new MockWordProcessor();

			// Act
			Program.ProcessWords(reader, processor);

			// Assert
			Assert.Equal(words, processor.Words);
			Assert.Equal(1, processor.FinishCallCounter);
		}
	}
}