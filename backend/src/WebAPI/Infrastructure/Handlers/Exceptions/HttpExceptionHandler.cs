using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Infrastructure.Handlers.ExceptionDetails;

namespace WebAPI.Infrastructure.Handlers.Exceptions;

public sealed class HttpExceptionHandler : ExceptionHandler
{
    private HttpResponse? _httpResponse;

    public HttpResponse HttpResponse
    {
        get => _httpResponse ?? throw new ArgumentNullException(nameof(_httpResponse));
        set => _httpResponse = value;
    }

    public override async Task HandleExceptionAsync(Exception e)
    {
        HttpResponse.ContentType = "application/json";

        ProblemDetails response = e switch
        {
            BusinessException businessException => new BusinessExceptionDetails(businessException.Message),
            UnauthorizedAccessException unauthorizedAccessException => new UnauthorizedAccessExceptionDetails(unauthorizedAccessException.Message),
            DatabaseOperationFailedException databaseOperationFailedException => new DatabaseOperationFailedExceptionDetails(databaseOperationFailedException.Message),
            _ => new UnhandledExceptionDetails(e.Message)
        };

        HttpResponse.StatusCode = (int)response.Status!;

        await HttpResponse.WriteAsJsonAsync(response);
    }

}