namespace Domain.Errors;

public readonly record struct OperationClaimErrors
{
    public static readonly Error NameCannotBeEmpty =
        Error.Create("OperationClaim.NameCannotBeEmpty", "Operation claim name cannot be empty");

    public static readonly Error NameCannotBeLongerThan50Characters = Error.Create(
        "OperationClaim.NameCannotBeLongerThan50Characters",
        "Operation claim name cannot be longer than 50 characters");

    public static readonly Error NameCannotBeShorterThan2Characters = Error.Create(
        "OperationClaim.NameCannotBeShorterThan2Characters",
        "Operation claim name cannot be shorter than 2 characters");

    public static readonly Error NameCannotContainWhiteSpaces =
        Error.Create("OperationClaim.NameCannotContainWhiteSpaces", "Operation claim name cannot contain white spaces");

    public static readonly Error NameCannotContainSpecialCharacters = Error.Create(
        "OperationClaim.NameCannotContainSpecialCharacters", "Operation claim name cannot contain special characters");

    public static readonly Error NameCannotBeDuplicated = Error.Create("OperationClaim.NameCannotBeDuplicated",
        "Operation claim name cannot be duplicated");
    
    public static readonly Error NotFound = Error.Create("OperationClaim.NotFound",
        "Operation claim not found");
}