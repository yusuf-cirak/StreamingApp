using Microsoft.AspNetCore.Mvc;

namespace SharedKernel;

public interface IErrorResponse
{
    int StatusCode { get; }

    ProblemDetails ToProblemDetails();
}