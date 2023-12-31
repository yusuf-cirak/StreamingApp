namespace Domain.Errors;

public readonly record struct StreamBlockedUserErrors
{
    public static readonly Error UserIdCannotBeEmpty =
        Error.Create("StreamBlockedUser.UserIdCannotBeEmpty", "Stream blocked user user id cannot be empty");

    public static readonly Error FailedToBlockUser =
        Error.Create("StreamBlockedUser.FailedToBlockUser", "Failed to block user");

    public static readonly Error UserIsAlreadyBlocked =
        Error.Create("StreamBlockedUser.UserIsAlreadyBlocked", "User is already blocked from the stream");

    public static readonly Error FailedToUnblockUser =
        Error.Create("StreamBlockedUser.FailedToUnblockUser", "Failed to remove block from user");

    public static readonly Error UserIdCannotBeLongerThan100Characters = Error.Create(
        "StreamBlockedUser.UserIdCannotBeLongerThan100Characters",
        "Stream blocked user user id cannot be longer than 100 characters");

    public static readonly Error UserIdCannotBeShorterThan2Characters = Error.Create(
        "StreamBlockedUser.UserIdCannotBeShorterThan2Characters",
        "Stream blocked user user id cannot be shorter than 2 characters");

    public static readonly Error UserIdCannotContainWhiteSpaces =
        Error.Create("StreamBlockedUser.UserIdCannotContainWhiteSpaces",
            "Stream blocked user user id cannot contain white spaces");

    public static readonly Error UserIdCannotContainSpecialCharacters = Error.Create(
        "StreamBlockedUser.UserIdCannotContainSpecialCharacters",
        "Stream blocked user user id cannot contain special characters");

    public static readonly Error UserIdCannotBeDuplicated = Error.Create("StreamBlockedUser.UserIdCannotBeDuplicated",
        "Stream blocked user user id cannot be duplicated");
}