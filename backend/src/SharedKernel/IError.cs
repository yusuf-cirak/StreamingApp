using Microsoft.AspNetCore.Mvc;

namespace SharedKernel;

public interface IError
{
    int StatusCode { get; }

    ProblemDetails ToProblemDetails();
}