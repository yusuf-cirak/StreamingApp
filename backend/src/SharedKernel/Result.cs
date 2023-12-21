namespace SharedKernel;

public readonly record struct Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    private Result(TValue value)
    {
        _value = value;
        _error = default;
        IsSuccess = true;
    }

    private Result(TError error)
    {
        _value = default;
        _error = error;
        IsSuccess = false;
    }

    public static Result<TValue, TError> Ok(TValue value) => new(value);

    public static Result<TValue, TError> Fail(TError error) => new(error);

    public static implicit operator Result<TValue, TError>(TValue value) => Ok(value);

    public static implicit operator Result<TValue, TError>(TError error) => Fail(error);

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> failure)
        => IsSuccess ? success(_value!) : failure(_error!);
}