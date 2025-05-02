namespace wsm.Exceptions;

public class PermissionsException : Exception
{
    public PermissionsException() : base("You do not have the necessary permissions to perform this operation.")
    {
    }

    public PermissionsException(string message) : base(message)
    {
    }

    public PermissionsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}