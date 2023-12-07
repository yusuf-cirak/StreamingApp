namespace SharedKernel;

public readonly record struct Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    private Result(bool isSuccess, Error error)
    {
        if (isSuccess && error!=Error.None || !isSuccess && error==Error.None)
        {
            throw new ArgumentException("Invalid error",nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}
