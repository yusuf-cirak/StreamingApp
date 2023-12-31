﻿using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Infrastructure.Handlers.ExceptionDetails;

public sealed class DatabaseOperationFailedExceptionDetails : ProblemDetails
{
    public DatabaseOperationFailedExceptionDetails(string detail)
    {
        Status = StatusCodes.Status500InternalServerError;
        Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
        Detail = detail;
        Title = "Database Operation Error";
    }
}