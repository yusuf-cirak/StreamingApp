namespace Application.Common.Errors;

public readonly record struct ValidationErrors
{
    public static readonly Error InvalidRequest = Error.Create("Error.InvalidRequest", "Invalid request", 400);

    public static Error InvalidCredentials(string message, int statusCode = 400) =>
        Error.Create("Error.InvalidCredentials", message, statusCode);
}