using Microsoft.AspNetCore.Mvc;

namespace SharedKernel;

public readonly record struct Error : IError
{
    public string Code { get; }
    public string Message { get; }
    public int StatusCode { get; }

    private Error(string code, string message, int statusCode)
    {
        Code = code;
        Message = message;
        StatusCode = statusCode;
    }


    public ProblemDetails ToProblemDetails() => new ProblemDetails()
    {
        Status = StatusCode,
        Title = Code,
        Detail = Message
    };


    public static readonly Error None = new(string.Empty, string.Empty, 0);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided", 500);

    public static Error Create(string code, string message, int statusCode = 400) => new(code, message, statusCode);


    public static implicit operator Result(Error error) => Result.Failure(error);
}