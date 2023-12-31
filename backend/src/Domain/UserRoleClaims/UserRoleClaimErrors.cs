namespace Domain.Errors;

public readonly record struct UserRoleClaimErrors
{
    public static readonly Error NameCannotBeEmpty =
        Error.Create("UserRoleClaim.NameCannotBeEmpty", "User role claim name cannot be empty");

    public static readonly Error FailedToCreate =
        Error.Create("UserRoleClaim.FailedToCreate", "Failed to create user role claim");

    public static readonly Error FailedToDelete =
        Error.Create("UserRoleClaim.FailedToDelete", "Failed to delete user role claim");

    public static readonly Error NameCannotBeLongerThan50Characters = Error.Create(
        "UserRoleClaim.NameCannotBeLongerThan50Characters",
        "User role claim name cannot be longer than 50 characters");

    public static readonly Error NameCannotBeShorterThan2Characters = Error.Create(
        "UserRoleClaim.NameCannotBeShorterThan2Characters",
        "User role claim name cannot be shorter than 2 characters");

    public static readonly Error NameCannotContainWhiteSpaces =
        Error.Create("UserRoleClaim.NameCannotContainWhiteSpaces", "User role claim name cannot contain white spaces");

    public static readonly Error NameCannotContainSpecialCharacters = Error.Create(
        "UserRoleClaim.NameCannotContainSpecialCharacters", "User role claim name cannot contain special characters");

    public static readonly Error NameCannotBeDuplicated = Error.Create("UserRoleClaim.NameCannotBeDuplicated",
        "User role claim name cannot be duplicated");
}