namespace Application.Common.Exceptions;

public sealed class AuthorizationException : Exception
{
    public AuthorizationException(string message) : base(message)
    {

    }

    public AuthorizationException(string message, Exception exception) : base(message, exception)
    {

    }
}