namespace SharedKernel;
public readonly record struct Error(string Code, string Description)
{

    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NullValue = new("Error.NullValue", "Value cannot be null");

    public static readonly Error NotFound = new("Error.NotFound", "Value not found");


    public static implicit operator Result(Error error) => Result.Failure(error);

    public Result ToResult() => Result.Failure(this);


}
