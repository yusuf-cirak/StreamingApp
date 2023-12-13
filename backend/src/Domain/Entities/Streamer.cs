namespace Domain.Entities;

public class Streamer : Entity
{
    public string StreamKey { get; set; }
    public string StreamTitle { get; set; } = string.Empty;
    public string StreamDescription { get; set; } = string.Empty;

    public virtual User User { get; set; }

    private Streamer(Guid userId, string streamKey)
    {
        Id = userId;
        StreamKey = streamKey;
    }


    public static Streamer Create(Guid userId, string streamKey)
    {
        Streamer streamer = new(userId, streamKey);

        streamer.Raise(new StreamerCreatedEvent(streamer));

        return streamer;
    }
}