namespace Application.Common.Errors;

public readonly record struct AuthorizationErrors
{
    public static Error Unauthorized() => Error.Create("Error.Unauthorized", "Unauthorized", 401);
    public static Error Unauthorized(string message) => Error.Create("Error.Unauthorized", message, 401);
    public static readonly Error Forbidden = Error.Create("Error.Forbidden", "Forbidden", 403);
}