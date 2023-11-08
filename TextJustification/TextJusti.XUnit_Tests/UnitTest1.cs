namespace TextJusti.XUnit_Tests;

public class UnitTest1
{
    private const string ArgError = "Argument Error";
    private const string FileError = "File Error";
    
    [Fact]
    public void NoArguments()
    {
        // Arrange
        string[] args = {};
        
        // Act
        // Assert

        Assert.Equal(ArgError,Console.Out.ToString());

    }
}
