namespace Domain.Errors;

public readonly record struct StreamErrors
{
    public static readonly Error NameCannotBeEmpty =
        Error.Create("Stream.NameCannotBeEmpty", "Stream name cannot be empty");

    public static readonly Error FailedToCreateStream =
        Error.Create("Stream.FailedToCreateStream", "Failed to create stream");

    public static readonly Error NameCannotBeLongerThan50Characters = Error.Create(
        "Stream.NameCannotBeLongerThan50Characters",
        "Stream name cannot be longer than 50 characters");

    public static readonly Error NameCannotBeShorterThan2Characters = Error.Create(
        "Stream.NameCannotBeShorterThan2Characters",
        "Stream name cannot be shorter than 2 characters");

    public static readonly Error NameCannotContainWhiteSpaces =
        Error.Create("Stream.NameCannotContainWhiteSpaces", "Stream name cannot contain white spaces");

    public static readonly Error NameCannotContainSpecialCharacters = Error.Create(
        "Stream.NameCannotContainSpecialCharacters", "Stream name cannot contain special characters");

    public static readonly Error NameCannotBeDuplicated = Error.Create("Stream.NameCannotBeDuplicated",
        "Stream name cannot be duplicated");

    public static readonly Error UserIsNotModeratorOfStream = Error.Create("Stream.UserIsNotModeratorOfStream",
        "User is not moderator of the stream");

    public static readonly Error UserIsNotStreamer = Error.Create("Stream.UserIsNotStreamer",
        "User is not the streamer");
    
    public static readonly Error StreamerNotExists = Error.Create("Stream.StreamerNotExists",
        "Streamer does not exist");
}