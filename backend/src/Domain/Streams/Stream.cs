namespace Domain.Entities;

public class Stream : Entity
{
    public Guid StreamerId { get; set; }
    public bool ChatDisabled { get; set; } = false;
    public int ChatDelaySecond { get; set; } = 0;
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public virtual Streamer Streamer { get; set; }

    private Stream()
    {
    }

    private Stream(Guid streamerId)
    {
        StreamerId = streamerId;
        StartedAt = DateTime.UtcNow;
    }

    public static Stream Create(Guid streamerId) => new(streamerId);
}