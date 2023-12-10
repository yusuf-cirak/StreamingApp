namespace WebAPI.Infrastructure.Handlers.Exceptions;

public abstract class ExceptionHandler
{
    public abstract Task HandleExceptionAsync(Exception exception);

}