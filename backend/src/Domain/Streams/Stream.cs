namespace Domain.Entities;

public class Stream : Entity
{
    public Guid StreamerId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public virtual Streamer Streamer { get; set; }

    private Stream()
    {
        StartedAt = DateTime.UtcNow;
    }

    private Stream(Guid streamerId) : this()
    {
        StreamerId = streamerId;
    }
    
    private Stream(Guid streamerId, DateTime startedAt)
    {
        StreamerId = streamerId;
        StartedAt = startedAt;
    }

    public static Stream Create(Guid streamerId) => new(streamerId);
    public static Stream Create(Guid streamerId, DateTime startedAt) => new(streamerId, startedAt);
}