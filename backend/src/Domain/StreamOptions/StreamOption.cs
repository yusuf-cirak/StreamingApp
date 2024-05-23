namespace Domain.Entities;

public class StreamOption : Entity
{
    public string StreamKey { get; set; } = string.Empty;
    public string StreamTitle { get; set; } = string.Empty;
    public string StreamDescription { get; set; } = string.Empty;

    public string ThumbnailUrl { get; set; } = string.Empty;

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

    private StreamOption(Guid userId, string streamKey, string streamTitle, string streamDescription,
        bool mustBeFollower, bool chatDisabled, int chatDelaySecond) : base(userId)
    {
        StreamKey = streamKey;
        StreamTitle = streamTitle;
        StreamDescription = streamDescription;
        MustBeFollower = mustBeFollower;
        ChatDisabled = chatDisabled;
        ChatDelaySecond = chatDelaySecond;
    }


    private StreamOption(Guid userId, string streamKey, string streamTitle, string streamDescription,
        bool mustBeFollower, bool chatDisabled, int chatDelaySecond, string thumbnailUrl) : base(userId)
    {
        StreamKey = streamKey;
        StreamTitle = streamTitle;
        StreamDescription = streamDescription;
        MustBeFollower = mustBeFollower;
        ChatDisabled = chatDisabled;
        ChatDelaySecond = chatDelaySecond;
        ThumbnailUrl = thumbnailUrl;
    }


    public static StreamOption Create(Guid userId, string streamKey, string streamTitle, string streamDescription)
    {
        StreamOption streamOption = new(userId, streamKey, streamTitle, streamDescription);
        return streamOption;
    }

    public static StreamOption Create(Guid userId, string streamKey, string streamTitle, string streamDescription,
        bool mustBeFollower, bool chatDisabled, int chatDelaySecond)
    {
        StreamOption streamOption = new(userId, streamKey, streamTitle, streamDescription, mustBeFollower, chatDisabled,
            chatDelaySecond);
        return streamOption;
    }

    public static StreamOption Create(Guid userId, string streamKey, string streamTitle, string streamDescription,
        bool mustBeFollower, bool chatDisabled, int chatDelaySecond, string thumbnailUrl)
    {
        StreamOption streamOption = new(userId, streamKey, streamTitle, streamDescription, mustBeFollower, chatDisabled,
            chatDelaySecond, thumbnailUrl);
        return streamOption;
    }

    public StreamOption Update(bool mustBeFollower, bool chatDisabled, int chatDelaySecond)
    {
        this.MustBeFollower = mustBeFollower;
        this.ChatDisabled = chatDisabled;
        this.ChatDelaySecond = chatDelaySecond;

        return this;
    }

    public StreamOption Update(string streamTitle, string streamDescription)
    {
        this.StreamTitle = streamTitle;
        this.StreamDescription = streamDescription;

        return this;
    }

    public StreamOption Update(string streamTitle, string streamDescription, string thumbnailUrl)
    {
        this.StreamTitle = streamTitle;
        this.StreamDescription = streamDescription;
        this.ThumbnailUrl = thumbnailUrl;

        return this;
    }

    public StreamOption Clone()
    {
        return Create(Id, StreamKey, StreamTitle, StreamDescription, MustBeFollower, ChatDisabled,
            ChatDelaySecond, ThumbnailUrl);
    }
}