namespace wsm.Exceptions;

public class ServiceOperationException : Exception
{
    public ServiceOperationException() : base("An error occurred while accessing Windows API.")
    {
    }
    
    public ServiceOperationException(string message) : base(message)
    {
    }
}