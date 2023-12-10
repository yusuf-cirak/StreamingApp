using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Infrastructure.Handlers.ExceptionDetails;

public sealed class BusinessExceptionDetails : ProblemDetails
{
    public BusinessExceptionDetails(string detail)
    {
        Status = StatusCodes.Status400BadRequest;
        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        Detail = detail;
        Title = "Bad Request";
    }
}
