namespace Domain.Entities;

public class StreamModerator : BaseEntity
{
    public Guid StreamerId { get; set; }
    public Guid UserId { get; set; }
    
    public virtual Streamer Streamer { get; set; }
    public virtual User User { get; set; }
    
    private StreamModerator()
    {
    }
    
    private StreamModerator(Guid streamerId, Guid userId)
    {
        StreamerId = streamerId;
        UserId = userId;
    }
    
    public static StreamModerator Create(Guid streamerId, Guid userId)
    {
        StreamModerator streamModerator = new(streamerId, userId);
        // streamModerator.Raise(new StreamModeratorCreatedEvent(streamModerator));
        return streamModerator;
    }
    
}