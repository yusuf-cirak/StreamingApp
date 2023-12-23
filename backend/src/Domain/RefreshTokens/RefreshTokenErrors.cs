namespace Domain.Errors;

public readonly record struct RefreshTokenErrors
{
    public static readonly Error TokenCannotBeEmpty =
        Error.Create("RefreshToken.TokenCannotBeEmpty", "Refresh token cannot be empty");

    public static readonly Error TokenCannotBeLongerThan100Characters = Error.Create(
        "RefreshToken.TokenCannotBeLongerThan100Characters",
        "Refresh token cannot be longer than 100 characters");

    public static readonly Error TokenCannotBeShorterThan2Characters = Error.Create(
        "RefreshToken.TokenCannotBeShorterThan2Characters",
        "Refresh token cannot be shorter than 2 characters");

    public static readonly Error TokenCannotContainWhiteSpaces =
        Error.Create("RefreshToken.TokenCannotContainWhiteSpaces", "Refresh token cannot contain white spaces");

    public static readonly Error TokenCannotContainSpecialCharacters = Error.Create(
        "RefreshToken.TokenCannotContainSpecialCharacters", "Refresh token cannot contain special characters");

    public static readonly Error TokenCannotBeDuplicated = Error.Create("RefreshToken.TokenCannotBeDuplicated",
        "Refresh token cannot be duplicated");
    
    public static readonly Error TokenIsNotValid = Error.Create("RefreshToken.TokenIsNotValid",
        "Refresh token is not valid");
    
    public static readonly Error TokenIsExpired = Error.Create("RefreshToken.TokenIsExpired",
        "Refresh token is expired");
    
    public static readonly Error TokenNotFound = Error.Create("RefreshToken.TokenNotFound",
        "Refresh token is not found");
}