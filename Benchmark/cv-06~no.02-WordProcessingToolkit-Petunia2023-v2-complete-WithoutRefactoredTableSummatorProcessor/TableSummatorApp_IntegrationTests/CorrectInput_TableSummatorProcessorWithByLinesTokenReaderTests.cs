using TableSummatorApp;
using TokenProcessingFramework;

namespace TableSummatorApp_IntegrationTests {
	public class CorrectInput_TableSummatorProcessorWithByLinesTokenReaderTests {
		[Fact]
		public void SingleColumn_SingleRow() {
			// Arrange
			var input = """
				cena
				12

				""";
			var selectedColumn = "cena";
			var expectedOutput = """
				cena
				----
				12

				""";

			var tokenReader = new ByLinesTokenReader(new StringReader(input));
			var outputWriter = new StringWriter();
			var processor = new TableSummatorProcessor(outputWriter, selectedColumn);

			// Act
			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);

			// Assert
			Assert.Equal(expectedOutput, outputWriter.ToString());
		}

		[Fact]
		public void SingleColumn_MultipleRows() {
			// Arrange
			var input = """
				cena
				12
				10
				30

				""";
			var selectedColumn = "cena";
			var expectedOutput = """
				cena
				----
				52

				""";
			
			var tokenReader = new ByLinesTokenReader(new StringReader(input));
			var outputWriter = new StringWriter();
			var processor = new TableSummatorProcessor(outputWriter, selectedColumn);

			// Act
			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);

			// Assert
			Assert.Equal(expectedOutput, outputWriter.ToString());
		}

		[Fact]
		public void MultipleColumns_SelectedFirst_SingleRow_OneInterwordSpace() {
			// Arrange
			var input = """
				cena mesic zbozi typ prodejce
				1321 leden jablka dovoz Adamec

				""";
			var selectedColumn = "cena";
			var expectedOutput = """
				cena
				----
				1321

				""";

			var tokenReader = new ByLinesTokenReader(new StringReader(input));
			var outputWriter = new StringWriter();
			var processor = new TableSummatorProcessor(outputWriter, selectedColumn);

			// Act
			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);

			// Assert
			Assert.Equal(expectedOutput, outputWriter.ToString());
		}

		[Fact]
		public void MultipleColumns_SelectedThird_SingleRow_OneInterwordSpace() {
			// Arrange
			var input = """
				mesic zbozi cena typ prodejce
				leden jablka 1321 dovoz Adamec

				""";
			var selectedColumn = "cena";
			var expectedOutput = """
				cena
				----
				1321

				""";

			var tokenReader = new ByLinesTokenReader(new StringReader(input));
			var outputWriter = new StringWriter();
			var processor = new TableSummatorProcessor(outputWriter, selectedColumn);

			// Act
			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);

			// Assert
			Assert.Equal(expectedOutput, outputWriter.ToString());
		}

		[Fact]
		public void MultipleColumns_SelectedLast_SingleRow_OneInterwordSpace() {
			// Arrange
			var input = """
				mesic zbozi typ prodejce cena
				leden jablka dovoz Adamec 1321

				""";
			var selectedColumn = "cena";
			var expectedOutput = """
				cena
				----
				1321

				""";

			var tokenReader = new ByLinesTokenReader(new StringReader(input));
			var outputWriter = new StringWriter();
			var processor = new TableSummatorProcessor(outputWriter, selectedColumn);

			// Act
			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);

			// Assert
			Assert.Equal(expectedOutput, outputWriter.ToString());
		}

		[Fact]
		public void MultipleColumns_SelectedFirst_MultipleRows_OneInterwordSpace() {
			// Arrange
			var input = """
				cena mesic zbozi typ prodejce
				10895 leden brambory tuzemske Bartak
				15478 leden brambory vlastni Celestyn
				1321 leden jablka dovoz Adamec

				""";
			var selectedColumn = "cena";
			var expectedOutput = """
				cena
				----
				27694

				""";

			var tokenReader = new ByLinesTokenReader(new StringReader(input));
			var outputWriter = new StringWriter();
			var processor = new TableSummatorProcessor(outputWriter, selectedColumn);

			// Act
			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);

			// Assert
			Assert.Equal(expectedOutput, outputWriter.ToString());
		}

		[Fact]
		public void MultipleColumns_SelectedThird_MultipleRows_OneInterwordSpace() {
			// Arrange
			var input = """
				mesic zbozi cena typ prodejce
				leden brambory 10895 tuzemske Bartak
				leden brambory 15478 vlastni Celestyn
				leden jablka 1321 dovoz Adamec

				""";
			var selectedColumn = "cena";
			var expectedOutput = """
				cena
				----
				27694

				""";

			var tokenReader = new ByLinesTokenReader(new StringReader(input));
			var outputWriter = new StringWriter();
			var processor = new TableSummatorProcessor(outputWriter, selectedColumn);

			// Act
			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);

			// Assert
			Assert.Equal(expectedOutput, outputWriter.ToString());
		}

		[Fact]
		public void MultipleColumns_SelectedLast_MultipleRows_OneInterwordSpace() {
			// Arrange
			var input = """
				mesic zbozi typ prodejce cena
				leden brambory tuzemske Bartak 10895
				leden brambory vlastni Celestyn 15478
				leden jablka dovoz Adamec 1321

				""";
			var selectedColumn = "cena";
			var expectedOutput = """
				cena
				----
				27694

				""";

			var tokenReader = new ByLinesTokenReader(new StringReader(input));
			var outputWriter = new StringWriter();
			var processor = new TableSummatorProcessor(outputWriter, selectedColumn);

			// Act
			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);

			// Assert
			Assert.Equal(expectedOutput, outputWriter.ToString());
		}

		[Fact]
		public void ComplexTable_Tabs() {
			// Arrange
			var input = """
				mesic	zbozi	cena	typ	prodejce
				leden	brambory	10895	tuzemske	Bartak
				leden	brambory	15478	vlastni	Celestyn
				leden	jablka	1321	dovoz	Adamec

				""";
			var selectedColumn = "cena";
			var expectedOutput = """
				cena
				----
				27694

				""";

			var tokenReader = new ByLinesTokenReader(new StringReader(input));
			var outputWriter = new StringWriter();
			var processor = new TableSummatorProcessor(outputWriter, selectedColumn);

			// Act
			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);

			// Assert
			Assert.Equal(expectedOutput, outputWriter.ToString());
		}

		[Fact]
		public void ComplexTable_MultipleWhitespacesInHeader() {
			// Arrange
			var input = """
				mesic      zbozi		 cena   typ prodejce
				leden brambory 10895 tuzemske Bartak
				leden brambory 15478 vlastni Celestyn
				leden jablka 1321 dovoz Adamec

				""";
			var selectedColumn = "cena";
			var expectedOutput = """
				cena
				----
				27694

				""";

			var tokenReader = new ByLinesTokenReader(new StringReader(input));
			var outputWriter = new StringWriter();
			var processor = new TableSummatorProcessor(outputWriter, selectedColumn);

			// Act
			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);

			// Assert
			Assert.Equal(expectedOutput, outputWriter.ToString());
		}

		[Fact]
		public void ComplexTable_MultipleWhitespacesInBody() {
			// Arrange
			var input = """
				mesic zbozi cena typ prodejce
				leden brambory      10895 tuzemske Bartak
				leden			brambory 15478    vlastni								Celestyn
				leden jablka     1321 dovoz Adamec

				""";
			var selectedColumn = "cena";
			var expectedOutput = """
				cena
				----
				27694

				""";

			var tokenReader = new ByLinesTokenReader(new StringReader(input));
			var outputWriter = new StringWriter();
			var processor = new TableSummatorProcessor(outputWriter, selectedColumn);

			// Act
			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);

			// Assert
			Assert.Equal(expectedOutput, outputWriter.ToString());
		}

		[Fact]
		public void ComplexTable_MultipleWhitespacesInHeaderAndBody() {
			// Arrange
			var input = """
				mesic      zbozi		 cena   typ prodejce
				leden brambory      10895 tuzemske Bartak
				leden			brambory 15478    vlastni								Celestyn
				leden jablka     1321 dovoz Adamec

				""";
			var selectedColumn = "cena";
			var expectedOutput = """
				cena
				----
				27694

				""";

			var tokenReader = new ByLinesTokenReader(new StringReader(input));
			var outputWriter = new StringWriter();
			var processor = new TableSummatorProcessor(outputWriter, selectedColumn);

			// Act
			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);

			// Assert
			Assert.Equal(expectedOutput, outputWriter.ToString());
		}

		[Fact]
		public void ComplexTable_HeaderStartsWithWhitespaces() {
			// Arrange
			var input = """
						  mesic zbozi cena typ prodejce
				leden brambory 10895 tuzemske Bartak
				leden brambory 15478 vlastni Celestyn
				leden jablka 1321 dovoz Adamec

				""";
			var selectedColumn = "cena";
			var expectedOutput = """
				cena
				----
				27694

				""";

			var tokenReader = new ByLinesTokenReader(new StringReader(input));
			var outputWriter = new StringWriter();
			var processor = new TableSummatorProcessor(outputWriter, selectedColumn);

			// Act
			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);

			// Assert
			Assert.Equal(expectedOutput, outputWriter.ToString());
		}

		[Fact]
		public void ComplexTable_BodyRowStartsWithWhitespaces() {
			// Arrange
			var input = """
				mesic zbozi cena typ prodejce
				leden brambory 10895 tuzemske Bartak
									leden brambory 15478 vlastni Celestyn
				leden jablka 1321 dovoz Adamec

				""";
			var selectedColumn = "cena";
			var expectedOutput = """
				cena
				----
				27694

				""";

			var tokenReader = new ByLinesTokenReader(new StringReader(input));
			var outputWriter = new StringWriter();
			var processor = new TableSummatorProcessor(outputWriter, selectedColumn);

			// Act
			DefaultTokenProcessing.ProcessTokensUntilEndOfInput(tokenReader, processor);

			// Assert
			Assert.Equal(expectedOutput, outputWriter.ToString());
		}
	}
}