namespace SharedKernel;

public readonly record struct HttpResult
{
    public Error Error { get; } = Error.None;

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public int StatusCode { get; }

    private HttpResult(Error error)
    {
        Error = error;
        IsSuccess = false;
    }

    private HttpResult(bool isSuccess)
    {
        Error = default;
        IsSuccess = isSuccess;
    }

    public static HttpResult Success() => new(true);

    public static HttpResult Failure(Error error) => new(error);


    public static HttpResult Failure() => new(false);

    public static implicit operator HttpResult(Error error) => Failure(error);

    public TResult Match<TResult>(Func<TResult> success, Func<Error, TResult> failure)
        => IsSuccess ? success() : failure(Error);
}

public readonly record struct HttpResult<TValue>
{
    public TValue Value { get; }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public int StatusCode { get; }

    private HttpResult(TValue value, bool isSuccess, int statusCode)
    {
        Value = value;
        IsSuccess = isSuccess;
        StatusCode = statusCode;
    }

    public static HttpResult<TValue> Success(TValue value, int statusCode = 200) => new(value, true, statusCode);

    public static HttpResult<TValue> Failure(TValue error, int statusCode = 400) => new(error, false, statusCode);

    public TResult Match<TResult>(Func<TValue, int, TResult> success, Func<TValue, int, TResult> failure)
        => IsSuccess ? success(Value, StatusCode) : failure(Value, StatusCode);
}

public readonly record struct HttpResult<TValue, TError>
{
    public TValue Value { get; }

    public TError Error { get; }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public int StatusCode { get; }

    private HttpResult(TValue value, int statusCode)
    {
        Value = value;
        Error = default;
        IsSuccess = true;
        StatusCode = statusCode;
    }

    private HttpResult(TError error, int statusCode)
    {
        Value = default;
        Error = error;
        IsSuccess = false;
        StatusCode = statusCode;
    }


    public static HttpResult<TValue, TError> Success(TValue value, int statusCode = 200) => new(value, statusCode);

    public static HttpResult<TValue, TError> Failure(TError error, int statusCode = 400) =>
        new(error, statusCode);

    public static implicit operator HttpResult<TValue, TError>(TValue value) => Success(value);

    public static implicit operator HttpResult<TValue, TError>(TError error) => Failure(error);

    public TResult Match<TResult>(Func<TValue, int, TResult> success, Func<TError, int, TResult> failure)
        => IsSuccess ? success(Value, StatusCode) : failure(Error, StatusCode);
}