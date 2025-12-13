namespace Exceptions;

public class ParseServiceException : Exception
{
    public ParseServiceException()
    {
    }

    public ParseServiceException(string message) : base(message)
    {
    }

    public ParseServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}