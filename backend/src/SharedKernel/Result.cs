namespace SharedKernel;

public readonly record struct Result
{
    public Error Error { get; } = Error.None;

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    private Result(Error error)
    {
        Error = error;
        IsSuccess = false;
    }

    private Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public static Result Success() => new(true);

    public static Result Failure(Error error) => new(error);


    public static Result Failure() => new(false);

    public static implicit operator Result(Error error) => Failure(error);

    public TResult Match<TResult>(Func<TResult> success, Func<Error, TResult> failure)
        => IsSuccess ? success() : failure(Error);
}

public sealed record Result<TValue, TError> where TError : IError
{
    public TValue Value { get; } = default;
    public TError Error { get; } = default;

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    private Result(TValue value)
    {
        Value = value;
        IsSuccess = true;
    }

    private Result(TError error)
    {
        Error = error;
        IsSuccess = false;
    }

    private static Result<TValue, TError> Success(TValue value) => new(value);

    private static Result<TValue, TError> Failure(TError error) => new(error);

    public static implicit operator Result<TValue, TError>(TValue value) => Success(value);

    public static implicit operator Result<TValue, TError>(TError error) => Failure(error);

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> failure)
        => this.IsSuccess ? success(this.Value) : failure(this.Error);
}