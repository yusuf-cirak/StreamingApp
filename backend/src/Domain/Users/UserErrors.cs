namespace Domain.Errors;

public readonly record struct UserErrors
{
    public static readonly Error NameCannotBeEmpty =
        Error.Create("User.NameCannotBeEmpty", "User name cannot be empty");

    public static readonly Error UserDoesNotExist =
        Error.Create("User.DoesNotExist", "User does not exist");

    public static readonly Error PasswordIsNotUpdated =
        Error.Create("User.PasswordIsNotUpdated", "User password is not updated");

    public static readonly Error OldAndNewPasswordAreEqual =
        Error.Create("User.PasswordIsNotUpdated", "Old and new password are equal");

    public static readonly Error NameCannotBeLongerThan50Characters = Error.Create(
        "User.NameCannotBeLongerThan50Characters",
        "User name cannot be longer than 50 characters");

    public static readonly Error NameCannotBeShorterThan8Characters = Error.Create(
        "User.NameCannotBeShorterThan2Characters",
        "User name cannot be shorter than 8 characters");

    public static readonly Error PasswordCannotBeShorterThan8Characters = Error.Create(
        "User.PasswordCannotBeShorterThan8Characters",
        "Password cannot be shorter than 8 characters");

    public static readonly Error PasswordCannotBeLongerThan50Characters = Error.Create(
        "User.PasswordCannotBeLongerThan50Characters",
        "Password cannot be longer than 50 characters");

    public static readonly Error NameCannotContainWhiteSpaces =
        Error.Create("User.NameCannotContainWhiteSpaces", "User name cannot contain white spaces");

    public static readonly Error NameCannotContainSpecialCharacters = Error.Create(
        "User.NameCannotContainSpecialCharacters", "User name cannot contain special characters");

    public static readonly Error NameCannotBeDuplicated = Error.Create("User.NameCannotBeDuplicated",
        "User name cannot be duplicated");

    public static readonly Error NotFound = Error.Create("User.NotFound", "User not found");

    public static readonly Error WrongCredentials =
        Error.Create("User.WrongCredentials", "User credentials does not match");
}