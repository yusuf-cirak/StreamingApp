using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Infrastructure.Handlers.ExceptionDetails;

public sealed class UnauthorizedAccessExceptionDetails : ProblemDetails
{
    public UnauthorizedAccessExceptionDetails(string detail)
    {
        Status = StatusCodes.Status401Unauthorized;
        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3";
        Detail = detail;
        Title = "Unauthorized Access";
    }
}