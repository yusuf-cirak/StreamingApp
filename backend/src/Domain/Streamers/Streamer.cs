namespace Domain.Streamers;

public sealed class Streamer : Entity
{
    public Guid UserId { get; set; }
    public string StreamKey { get; set; }
    public string StreamTitle { get; set; } = string.Empty;
    public string StreamDescription { get; set; } = string.Empty;

    private Streamer(Guid userId,string streamKey)
    {
        UserId = userId;
        StreamKey = streamKey;
    }


    public static Streamer Create(Guid userId, string streamKey)
    {
        Streamer streamer = new(userId, streamKey);

        streamer.Raise(new StreamerCreatedEvent(streamer));

        return streamer;
    }
}