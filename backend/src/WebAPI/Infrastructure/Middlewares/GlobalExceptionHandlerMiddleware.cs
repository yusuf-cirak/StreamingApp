using Microsoft.AspNetCore.Diagnostics;
using WebAPI.Infrastructure.Handlers.Exceptions;

namespace WebAPI.Infrastructure.Middlewares;

public sealed class GlobalExceptionHandlerMiddleware : IExceptionHandler
{
    private readonly HttpExceptionHandler _httpExceptionHandler = new();
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _httpExceptionHandler.HttpResponse = httpContext.Response;

        await _httpExceptionHandler.HandleExceptionAsync(exception);

        return true;
    }
}