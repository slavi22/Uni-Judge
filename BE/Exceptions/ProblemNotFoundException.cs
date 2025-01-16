namespace BE.Exceptions;

public class ProblemNotFoundException(string message) : Exception(message);
