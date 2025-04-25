namespace BE.Common.Exceptions;

public class ProblemNotFoundException(string message) : Exception(message);
