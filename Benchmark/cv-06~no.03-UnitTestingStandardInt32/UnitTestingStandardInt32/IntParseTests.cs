#nullable disable

namespace UnitTestingStandardInt32 {
	public class IntParseTests {
		[Fact]
		public void Parse_ValidString_ShouldSucceed() {
			string numberStr = "42";
			int result = int.Parse(numberStr);
			Assert.Equal(42, result);
		}

		[Fact]
		public void Parse_InvalidString_ShouldThrowFormatException() {
			string invalidStr = "notanumber";
			Assert.Throws<FormatException>(() => int.Parse(invalidStr));
		}

		[Fact]
		public void Parse_NullString_ShouldThrowArgumentNullException() {
			string nullStr = null;
			Assert.Throws<ArgumentNullException>(() => int.Parse(nullStr));
		}

		[Fact]
		public void Parse_OverflowingString_ShouldThrowOverflowException() {
			string overflowStr = "2147483648"; // This is larger than Int32.MaxValue
			Assert.Throws<OverflowException>(() => int.Parse(overflowStr));
		}
	}
}