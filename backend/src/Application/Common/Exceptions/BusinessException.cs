namespace Application.Common.Exceptions;

public sealed class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {

    }

    public BusinessException(string message, Exception exception) : base(message, exception)
    {

    }
}
