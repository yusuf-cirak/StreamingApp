namespace Application.Common.Errors;

public readonly record struct AuthorizationErrors
{
    public static readonly Error Unauthorized = Error.Create("Error.Unauthorized", "Unauthorized", 401);
    public static readonly Error Forbidden = Error.Create("Error.Forbidden", "Forbidden", 403);
}