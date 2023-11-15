using System;
using System.IO;
using TableSummatorApp;
using TokenProcessingFramework;
using Xunit;

namespace TableSummatorApp.Tests
{
    public record class TestableTableSummatorProcessor : TableSummatorProcessor
    {
        public TestableTableSummatorProcessor(TextWriter outputWriter, string targetColumnName)
            : base(outputWriter, targetColumnName)
        {
        }

        // Expose internal state for testing purposes
        public bool FoundTargetColumn => GetFoundTargetColumn();
        public bool ProcessingColumnHeaders => GetProcessingColumnHeaders();
        public long Sum => GetSum();
    }

    public class TableSummatorProcessorTests
    {
        [Fact]
        public void ProcessToken_HeaderWordToken_FoundTargetColumn()
        {
            // Arrange
            var outputWriter = new StringWriter();
            var processor = new TestableTableSummatorProcessor(outputWriter, "ColumnName");

            // Act
            processor.ProcessToken(new Token(TokenType.Word, "ColumnName"));

            // Assert
            Assert.True(processor.FoundTargetColumn);
        }

        [Fact]
        public void ProcessToken_HeaderEndOfLineToken_ProcessingColumnHeadersSwitchedOff()
        {
            // Arrange
            var outputWriter = new StringWriter();
            var processor = new TestableTableSummatorProcessor(outputWriter, "ColumnName");

            // Act
            processor.ProcessToken(new Token(TokenType.EndOfLine, ""));

            // Assert
            Assert.False(processor.ProcessingColumnHeaders);
        }

        [Fact]
        public void ProcessToken_TableDataWordToken_SumUpdated()
        {
            // Arrange
            var outputWriter = new StringWriter();
            var processor = new TestableTableSummatorProcessor(outputWriter, "ColumnName");

            // Set up the processor to process table data
            processor.ProcessToken(new Token(TokenType.EndOfLine, ""));
            processor.ProcessToken(new Token(TokenType.Word, "ColumnName"));

            // Act
            processor.ProcessToken(new Token(TokenType.Word, "42"));

            // Assert
            Assert.Equal(42, processor.Sum);
        }

        [Fact]
        public void Finish_ProcessingColumnHeaders_ExceptionThrown()
        {
            // Arrange
            var outputWriter = new StringWriter();
            var processor = new TestableTableSummatorProcessor(outputWriter, "ColumnName");

            // Act & Assert
            Assert.Throws<InvalidFileFormatApplicationException>(() => processor.Finish());
        }
    }
}
