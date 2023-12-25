namespace SharedKernel;

public readonly record struct HttpResult : IHttpResult
{
    public Error Error { get; } = Error.None;

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public int StatusCode { get; }

    private HttpResult(Error error, int statusCode)
    {
        Error = error;
        IsSuccess = false;
        StatusCode = statusCode;
    }

    private HttpResult(bool isSuccess)
    {
        Error = default;
        IsSuccess = isSuccess;
    }

    public static HttpResult Success() => new(true);

    public static HttpResult Failure(Error error, int statusCode = 400) => new(error, statusCode);


    public static HttpResult Failure() => new(false);

    public static implicit operator HttpResult(Error error) => Failure(error);

    public TResult Match<TResult>(Func<TResult> success, Func<Error, TResult> failure)
        => IsSuccess ? success() : failure(Error);

    public IHttpResult CreateWith(Error error, int statusCode)
    {
        return Failure(error, statusCode);
    }
}

public readonly record struct HttpResult<TValue> : IHttpResult
{
    public TValue Value { get; }

    public Error Error { get;  }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public int StatusCode { get; init; }

    public HttpResult()
    {
    }

    private HttpResult(TValue value, int statusCode)
    {
        Value = value;
        Error = default;
        IsSuccess = true;
        StatusCode = statusCode;
    }

    private HttpResult(Error error, int statusCode)
    {
        Value = default;
        Error = error;
        IsSuccess = false;
        StatusCode = statusCode;
    }


    public static HttpResult<TValue> Success(TValue value, int statusCode = 200) => new(value, statusCode);

    public static HttpResult<TValue> Failure(Error error, int statusCode = 400) =>
        new(error, statusCode);

    public static implicit operator HttpResult<TValue>(TValue value) => Success(value);

    public static implicit operator HttpResult<TValue>(Error error) => Failure(error);

    public TResult Match<TResult>(Func<TValue, int, TResult> success, Func<Error, int, TResult> failure)
        => IsSuccess ? success(Value, StatusCode) : failure(Error, StatusCode);

    public IHttpResult CreateWith(Error error, int statusCode)
    {
        return Failure(error, statusCode);
    }
}