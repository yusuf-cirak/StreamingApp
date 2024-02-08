namespace Domain.Entities;

public class StreamOption : Entity, ICloneable
{
    public string StreamKey { get; set; } = string.Empty;
    public string StreamTitle { get; set; } = string.Empty;
    public string StreamDescription { get; init; } = string.Empty;

    public bool MustBeFollower { get; set; } = false;
    public bool ChatDisabled { get; set; } = false;

    public int ChatDelaySecond { get; set; } = 0;

    public virtual User Streamer { get; }


    public StreamOption()
    {
    }

    private StreamOption(Guid userId, string streamKey, string streamTitle, string streamDescription) : base(userId)
    {
        StreamKey = streamKey;
        StreamTitle = streamTitle;
        StreamDescription = streamDescription;
    }


    public static StreamOption Create(Guid userId, string streamKey, string streamTitle, string streamDescription)
    {
        StreamOption streamOption = new(userId, streamKey, streamTitle, streamDescription);

        return streamOption;
    }

    public object Clone()
    {
        return Create(Id, this.StreamKey, this.StreamTitle, this.StreamDescription);
    }
}