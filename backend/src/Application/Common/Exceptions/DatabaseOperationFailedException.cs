namespace Application.Common.Exceptions;

public sealed class DatabaseOperationFailedException : Exception
{
    public DatabaseOperationFailedException(string message) : base(message)
    {

    }

    public DatabaseOperationFailedException(string message, Exception exception) : base(message, exception)
    {

    }
}