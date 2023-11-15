using System;
using System.IO;
using TokenProcessingFramework;

#nullable enable

namespace TableSummatorApp
{
    public record class TableSummatorProcessor  : ITokenProcessor    {
        private readonly TextWriter _outputWriter;
        private readonly string _targetColumnName;
        private int _headerColumnCount;
        private int _currentColumn = 0;
        private bool _processingColumnHeaders = true;
        private bool _foundTargetColumn = false;
        private int _targetColumnIndex;
        private long _sum = 0;

		protected bool GetFoundTargetColumn() => _foundTargetColumn;
		protected bool GetProcessingColumnHeaders() => _processingColumnHeaders;
		protected long GetSum() => _sum;
        
		public TableSummatorProcessor(TextWriter outputWriter, string targetColumnName)
        {
            _outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
            _targetColumnName = targetColumnName ?? throw new ArgumentNullException(nameof(targetColumnName));
        }

        public void ProcessToken(Token token)
        {
            if (_processingColumnHeaders)
                ProcessHeaderToken(token);
            else
                ProcessTableDataToken(token);
        }

        public void Finish()
        {
            if (_processingColumnHeaders)
                throw new InvalidFileFormatApplicationException();

            _outputWriter.WriteLine(_targetColumnName);
            _outputWriter.WriteLine(new string('-', _targetColumnName.Length));
            _outputWriter.WriteLine(_sum);
        }

        private void ProcessHeaderToken(Token token)
        {
            switch (token.Type)
            {
                case TokenType.Word:
                    ProcessHeaderWordToken(token);
                    break;
                case TokenType.EndOfLine:
                    ProcessHeaderEndOfLineToken();
                    break;
                default:
                    throw new InvalidFileFormatApplicationException();
            }
        }

        private void ProcessHeaderWordToken(Token token)
        {
            if (!_foundTargetColumn && (token.Value == _targetColumnName))
            {
                _targetColumnIndex = _currentColumn;
                _foundTargetColumn = true;
            }

            _currentColumn++;
        }

        private void ProcessHeaderEndOfLineToken()
        {
            if (_currentColumn == 0)
                throw new InvalidFileFormatApplicationException();
            if (!_foundTargetColumn)
                throw new NonExistentColumnNameApplicationException();

            _headerColumnCount = _currentColumn;
            _currentColumn = 0;
            _processingColumnHeaders = false;
        }

        private void ProcessTableDataToken(Token token)
        {
            switch (token.Type)
            {
                case TokenType.Word:
                    ProcessTableDataWordToken(token);
                    break;
                case TokenType.EndOfLine:
                    ProcessTableDataEndOfLineToken();
                    break;
                default:
                    throw new InvalidFileFormatApplicationException();
            }
        }

        private void ProcessTableDataWordToken(Token token)
        {
            if (_currentColumn == _targetColumnIndex)
            {
               try{
					_sum += Convert.ToInt32(token.Value);
				}
				catch{
					throw new InvalidIntegerValueApplicationException();
				}
            }

            _currentColumn++;
        }

        private void ProcessTableDataEndOfLineToken()
        {
            if (_currentColumn != _headerColumnCount)
                throw new InvalidFileFormatApplicationException();

            _currentColumn = 0;
        }
    }
}
