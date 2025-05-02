namespace wsm.Exceptions;

public class UnexpectedException : Exception
{
    public UnexpectedException() : base("An unexpected error occurred.")
    {
    }

    public UnexpectedException(string message) : base(message)
    {
    }

    public UnexpectedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}