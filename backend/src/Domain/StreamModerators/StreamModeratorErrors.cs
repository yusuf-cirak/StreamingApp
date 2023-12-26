namespace Domain.Errors;

public readonly record struct StreamModeratorErrors
{
    public static readonly Error StreamerIdCannotBeEmpty =
        Error.Create("StreamModerators.StreamerIdCannotBeEmpty", "Stream moderators streamer id cannot be empty");

    public static readonly Error StreamerIdCannotBeLongerThan100Characters = Error.Create(
        "StreamModerators.StreamerIdCannotBeLongerThan100Characters",
        "Stream moderators streamer id cannot be longer than 100 characters");

    public static readonly Error StreamerIdCannotBeShorterThan2Characters = Error.Create(
        "StreamModerators.StreamerIdCannotBeShorterThan2Characters",
        "Stream moderators streamer id cannot be shorter than 2 characters");

    public static readonly Error UserIsNotModeratorOfStream = Error.Create(
        "StreamModerators.UserIsNotModeratorOfStream",
        "User is not moderator of stream");
}