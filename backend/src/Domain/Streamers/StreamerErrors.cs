namespace Domain.Users;

public static class StreamerErrors
{
    public static Error NotFound(Guid streamId) =>
    new Error("Streamer.NotFound", $"The streamer with '{streamId}' was not found.");
}
