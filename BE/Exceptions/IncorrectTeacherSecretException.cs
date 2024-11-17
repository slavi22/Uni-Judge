namespace BE.Exceptions;

public class IncorrectTeacherSecretException(string message) : Exception(message)
{
}