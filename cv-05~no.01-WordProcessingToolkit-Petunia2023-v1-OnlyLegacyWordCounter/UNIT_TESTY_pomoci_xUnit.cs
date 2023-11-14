/*
"UNIT TESTY pomocí xUnit"


namespace ParagraphDetectingTokenReader_xUnit_tests;

public class UnitTests
{
    [Fact]
    public void OnlyEndOfFile()
    {
        // Arrange
        Token[] input = { new Token(TokenType.EndOfInput) };
        var mockTokenRead = new TokenReaderMock(input);
        var mockTokenReaderDecorator = new ParagraphDetectingTokenReaderDecorator(mockTokenRead);
        var expectedOutput = new Token(TokenType.EndOfInput);

        // Act        
        var output = mockTokenRead.ReadToken();
        
        // Assert
        Assert.Equal(expectedOutput, output);
    }
    [Fact]
    public void FiveWordsFollowedByNewLine()
    {
        // Arrange
        Token[] input = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };
        var mockTokenRead = new TokenReaderMock(input);
        var mockTokenReaderDecorator = new ParagraphDetectingTokenReaderDecorator(mockTokenRead);

        Token[] expectedOutput = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };

        // Act
        LinkedList<Token> output = new LinkedList<Token>();
        Token outputToken;
        while((outputToken = mockTokenReaderDecorator.ReadToken()).Type!= TokenType.EndOfInput){
            output.AddLast(outputToken); 
        }
        output.AddLast(outputToken);
        
        // Assert
        Assert.Equal(expectedOutput, output.ToArray());    
    }
    
    [Fact]
    public void TwoParragraphsOneInBetween()
    {
        // Arrange
        Token[] input = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word), 
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };
        var mockTokenRead = new TokenReaderMock(input);
        var mockTokenReaderDecorator = new ParagraphDetectingTokenReaderDecorator(mockTokenRead);

        Token[] expectedOutput = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfParagraph),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };

        // Act
        LinkedList<Token> output = new LinkedList<Token>();
        Token outputToken;
        while((outputToken = mockTokenReaderDecorator.ReadToken()).Type!= TokenType.EndOfInput){
            output.AddLast(outputToken); 
        }
        output.AddLast(outputToken);
        
        // Assert
        Assert.Equal(expectedOutput, output.ToArray());    
    }
    [Fact]
    public void FiveParragraphsOneInBetween()
    {
        // Arrange
        Token[] input = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word), 
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };
        var mockTokenRead = new TokenReaderMock(input);
        var mockTokenReaderDecorator = new ParagraphDetectingTokenReaderDecorator(mockTokenRead);

        Token[] expectedOutput = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfParagraph),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };

        // Act
        LinkedList<Token> output = new LinkedList<Token>();
        Token outputToken;
        while((outputToken = mockTokenReaderDecorator.ReadToken()).Type!= TokenType.EndOfInput){
            output.AddLast(outputToken); 
        }
        output.AddLast(outputToken);
        
        // Assert
        Assert.Equal(expectedOutput, output.ToArray());    
    }

    [Fact]
    public void StartWithNewLine()
    {
        // Arrange
        Token[] input = { 
            new Token(TokenType.EndOfLine),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };
        var mockTokenRead = new TokenReaderMock(input);
        var mockTokenReaderDecorator = new ParagraphDetectingTokenReaderDecorator(mockTokenRead);

        Token[] expectedOutput = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };

        // Act
        LinkedList<Token> output = new LinkedList<Token>();
        Token outputToken;
        while((outputToken = mockTokenReaderDecorator.ReadToken()).Type!= TokenType.EndOfInput){
            output.AddLast(outputToken); 
        }
        output.AddLast(outputToken);
        
        // Assert
        Assert.Equal(expectedOutput, output.ToArray());    
    }

    [Fact]
    public void StartWithFiveNewLine()
    {
        // Arrange
        Token[] input = { 
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };
        var mockTokenRead = new TokenReaderMock(input);
        var mockTokenReaderDecorator = new ParagraphDetectingTokenReaderDecorator(mockTokenRead);

        Token[] expectedOutput = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };

        // Act
        LinkedList<Token> output = new LinkedList<Token>();
        Token outputToken;
        while((outputToken = mockTokenReaderDecorator.ReadToken()).Type!= TokenType.EndOfInput){
            output.AddLast(outputToken); 
        }
        output.AddLast(outputToken);
        
        // Assert
        Assert.Equal(expectedOutput, output.ToArray());    
    }

    [Fact]
    public void MultipleParragraphsOneLineInBetween()
    {
        // Arrange
        Token[] input = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word), 
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),  
            new Token(TokenType.Word),
            new Token(TokenType.Word), 
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word), 
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };
        var mockTokenRead = new TokenReaderMock(input);
        var mockTokenReaderDecorator = new ParagraphDetectingTokenReaderDecorator(mockTokenRead);

        Token[] expectedOutput = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfParagraph),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfParagraph),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word), 
            new Token(TokenType.EndOfParagraph),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };

        // Act
        LinkedList<Token> output = new LinkedList<Token>();
        Token outputToken;
        while((outputToken = mockTokenReaderDecorator.ReadToken()).Type!= TokenType.EndOfInput){
            output.AddLast(outputToken); 
        }
        output.AddLast(outputToken);
        
        // Assert
        Assert.Equal(expectedOutput, output.ToArray());    
    }
    [Fact]
    public void MultipleParragraphsMultipleLineInBetween()
    {
        // Arrange
        Token[] input = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word), 
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),  
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word), 
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };
        var mockTokenRead = new TokenReaderMock(input);
        var mockTokenReaderDecorator = new ParagraphDetectingTokenReaderDecorator(mockTokenRead);

        Token[] expectedOutput = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfParagraph),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfParagraph),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word), 
            new Token(TokenType.EndOfParagraph),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };

        // Act
        LinkedList<Token> output = new LinkedList<Token>();
        Token outputToken;
        while((outputToken = mockTokenReaderDecorator.ReadToken()).Type!= TokenType.EndOfInput){
            output.AddLast(outputToken); 
        }
        output.AddLast(outputToken);
        
        // Assert
        Assert.Equal(expectedOutput, output.ToArray());    
    }
    [Fact]
    public void TrailingNewLine()
    {
        // Arrange
        Token[] input = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word), 
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };
        var mockTokenRead = new TokenReaderMock(input);
        var mockTokenReaderDecorator = new ParagraphDetectingTokenReaderDecorator(mockTokenRead);

        Token[] expectedOutput = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };

        // Act
        LinkedList<Token> output = new LinkedList<Token>();
        Token outputToken;
        while((outputToken = mockTokenReaderDecorator.ReadToken()).Type!= TokenType.EndOfInput){
            output.AddLast(outputToken); 
        }
        output.AddLast(outputToken);
        
        // Assert
        Assert.Equal(expectedOutput, output.ToArray());    
    }
    [Fact]
    public void FiveTrailingNewLines()
    {
        // Arrange
        Token[] input = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word), 
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };
        var mockTokenRead = new TokenReaderMock(input);
        var mockTokenReaderDecorator = new ParagraphDetectingTokenReaderDecorator(mockTokenRead);

        Token[] expectedOutput = { 
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.Word),
            new Token(TokenType.EndOfLine),
            new Token(TokenType.EndOfInput)
        };

        // Act
        LinkedList<Token> output = new LinkedList<Token>();
        Token outputToken;
        while((outputToken = mockTokenReaderDecorator.ReadToken()).Type!= TokenType.EndOfInput){
            output.AddLast(outputToken); 
        }
        output.AddLast(outputToken);
        
        // Assert
        Assert.Equal(expectedOutput, output.ToArray());    
    }


}

public class TokenReaderMock : ITokenReader
{
   private Token[] tokenArray;
   private int index; 
   public Token ReadToken(){
       index++;
       return tokenArray[index-1];
   }

   public TokenReaderMock(Token[] mockedArray){
        tokenArray = mockedArray;
        index = 0;
   }
}
*/
