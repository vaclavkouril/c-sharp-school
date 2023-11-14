using System;

namespace TableSummatorApp;

#nullable enable

public class InvalidFileFormatApplicationException : ApplicationException { }
public class InvalidIntegerValueApplicationException : ApplicationException { }
public class NonExistentColumnNameApplicationException : ApplicationException { }