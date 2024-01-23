namespace Domain.Errors;

public readonly record struct StreamOptionErrors
{
    public static readonly Error NameCannotBeEmpty =
        Error.Create("Streamer.NameCannotBeEmpty", "Streamer name cannot be empty");

    public static readonly Error NameCannotBeLongerThan50Characters = Error.Create(
        "Streamer.NameCannotBeLongerThan50Characters",
        "Streamer name cannot be longer than 50 characters");

    public static readonly Error NameCannotBeShorterThan2Characters = Error.Create(
        "Streamer.NameCannotBeShorterThan2Characters",
        "Streamer name cannot be shorter than 2 characters");

    public static readonly Error NameCannotContainWhiteSpaces =
        Error.Create("Streamer.NameCannotContainWhiteSpaces", "Streamer name cannot contain white spaces");

    public static readonly Error NameCannotContainSpecialCharacters = Error.Create(
        "Streamer.NameCannotContainSpecialCharacters", "Streamer name cannot contain special characters");

    public static readonly Error NameCannotBeDuplicated = Error.Create("Streamer.NameCannotBeDuplicated",
        "Streamer name cannot be duplicated");

    public static readonly Error CannotBeUpdated = Error.Create("Streamer.StreamerCannotBeUpdated",
        "Streamer cannot be updated");
    public static readonly Error StreamerDoesNotExist = Error.Create("Streamer.StreamerDoesNotExist",
        "Streamer does not exist");
}