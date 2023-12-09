namespace Domain.Entities;

public class Streamer : Entity
{
    public string StreamKey { get; set; }
    public string StreamTitle { get; set; } = string.Empty;
    public string StreamDescription { get; set; } = string.Empty;

    public virtual User User { get; set; }
    private Streamer(string streamKey)
    {
        StreamKey = streamKey;
    }


    public static Streamer Create(string streamKey)
    {
        Streamer streamer = new(streamKey);

        streamer.Raise(new StreamerCreatedEvent(streamer));

        return streamer;
    }
}