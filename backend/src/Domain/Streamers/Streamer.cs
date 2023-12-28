namespace Domain.Entities;

public class Streamer : Entity
{
    public string StreamKey { get; init; }
    public string StreamTitle { get; init; } = string.Empty;
    public string StreamDescription { get; init; } = string.Empty;

    public virtual User User { get; }


    public Streamer()
    {
    }

    private Streamer(Guid userId, string streamKey, string streamTitle, string streamDescription) : base(userId)
    {
        StreamKey = streamKey;
        StreamTitle = streamTitle;
        StreamDescription = streamDescription;
    }


    public static Streamer Create(Guid userId, string streamKey, string streamTitle, string streamDescription)
    {
        Streamer streamer = new(userId, streamKey, streamTitle, streamDescription);

        streamer.Raise(new StreamerCreatedEvent(streamer));

        return streamer;
    }
}