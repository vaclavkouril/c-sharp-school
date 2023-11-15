using System;
using System.IO;

using TokenProcessingFramework;

#nullable enable

namespace TableSummatorApp;

public record class TableSummatorProcessor(TextWriter OutputWriter, string TargetColumnName) : ITokenProcessor {
	private bool _processingColumnHeaders = true;
	private int _headerColumnCount;
	private int _currentColumn = 0;

	private bool _foundTargetColumn = false;
	private int _targetColumnIndex;
	private long _sum = 0;

	public void ProcessToken(Token token) {
		if (_processingColumnHeaders) {
			ProcessHeaderToken(token);
		} else {
			ProcessTableDataToken(token);
		}
	}

	private void ProcessHeaderToken(Token token) {
		switch (token.Type) {
			case TokenType.Word:
				if (!_foundTargetColumn && StringComparer.CurrentCultureIgnoreCase.Compare(token.Value, TargetColumnName) == 0) {
					_targetColumnIndex = _currentColumn;
					_foundTargetColumn = true;
				}
				_currentColumn++;
				break;
			case TokenType.EndOfLine:
				if (_currentColumn == 0) {
					throw new InvalidFileFormatApplicationException();
				} else if (!_foundTargetColumn) {
					throw new NonExistentColumnNameApplicationException();
				}
				_headerColumnCount = _currentColumn;
				_currentColumn = 0;
				_processingColumnHeaders = false;
				break;
			default:
				throw new InvalidFileFormatApplicationException();
		}
	}

	private void ProcessTableDataToken(Token token) {
		switch (token.Type) {
			case TokenType.Word:
				if (_currentColumn == _targetColumnIndex) {
					if (int.TryParse(token.Value!, out int value)) {
						_sum += value;
					} else {
						throw new InvalidIntegerValueApplicationException();
					}
				}
				_currentColumn++;
				break;
			case TokenType.EndOfLine:
				if (_currentColumn == 0 || _currentColumn != _headerColumnCount) {
					throw new InvalidFileFormatApplicationException();
				}
				_currentColumn = 0;
				break;
			default:
				throw new InvalidFileFormatApplicationException();
		}
	}

	public void Finish() {
		if (_processingColumnHeaders) {
			throw new InvalidFileFormatApplicationException();
		}
		OutputWriter.WriteLine(TargetColumnName);
		OutputWriter.WriteLine(new string('-', TargetColumnName.Length));
		OutputWriter.WriteLine(_sum);
	}
}
