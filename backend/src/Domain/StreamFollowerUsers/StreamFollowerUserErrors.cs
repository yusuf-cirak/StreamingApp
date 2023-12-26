namespace Domain.Errors;

public readonly record struct StreamFollowerUserErrors
{
    public static readonly Error StreamIdCannotBeEmpty =
        Error.Create("StreamFollowerUsers.StreamIdCannotBeEmpty", "Stream follower users stream id cannot be empty");

    public static readonly Error StreamIdCannotBeLongerThan100Characters = Error.Create(
        "StreamFollowerUsers.StreamIdCannotBeLongerThan100Characters",
        "Stream follower users stream id cannot be longer than 100 characters");

    public static readonly Error StreamIdCannotBeShorterThan2Characters = Error.Create(
        "StreamFollowerUsers.StreamIdCannotBeShorterThan2Characters",
        "Stream follower users stream id cannot be shorter than 2 characters");

    public static readonly Error StreamIdCannotContainWhiteSpaces =
        Error.Create("StreamFollowerUsers.StreamIdCannotContainWhiteSpaces",
            "Stream follower users stream id cannot contain white spaces");

    public static readonly Error StreamIdCannotContainSpecialCharacters = Error.Create(
        "StreamFollowerUsers.StreamIdCannotContainSpecialCharacters",
        "Stream follower users stream id cannot contain special characters");

    public static readonly Error StreamIdCannotBeDuplicated = Error.Create(
        "StreamFollowerUsers.StreamIdCannotBeDuplicated",
        "Stream follower users stream id cannot be duplicated");

    public static readonly Error UserIdCannotBeEmpty =
        Error.Create("StreamFollowerUsers.UserIdCannotBeEmpty", "Stream follower users user id cannot be empty");

    public static readonly Error UserIdCannotBeLongerThan100Characters = Error.Create(
        "StreamFollowerUsers.UserIdCannotBeLongerThan100Characters",
        "Stream follower users user id cannot be longer than 100 characters");

    public static readonly Error UserIdCannotBeShorterThan2Characters = Error.Create(
        "StreamFollowerUsers.UserIdCannotBeShorterThan2Characters",
        "Stream follower users user id cannot be shorter than 2 characters");

    public static readonly Error UserIdCannotContainWhiteSpaces =
        Error.Create("StreamFollowerUsers.UserIdCannotContainWhiteSpaces",
            "Stream follower users user id cannot contain white spaces");

    public static readonly Error UserIdCannotContainSpecialCharacters = Error.Create(
        "StreamFollowerUsers.UserIdCannotContainSpecialCharacters",
        "Stream follower users user id cannot contain special characters");

    public static readonly Error UserIdCannotBeDuplicated = Error.Create("StreamFollowerUsers.UserIdCannotBeDuplicated",
        "Stream follower users user id cannot be duplicated");
}