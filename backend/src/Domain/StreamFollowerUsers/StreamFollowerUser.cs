namespace Domain.Entities;

public class StreamFollowerUser : BaseEntity
{
    public Guid StreamerId { get; set; }
    public Guid UserId { get; set; }
    public virtual User Streamer { get; set; }
    public virtual User User { get; set; }
    
    private StreamFollowerUser()
    {
    }
    
    private StreamFollowerUser(Guid streamerId, Guid userId)
    {
        StreamerId = streamerId;
        UserId = userId;
    }
    
    public static StreamFollowerUser Create(Guid streamerId, Guid userId)
    {
        var follower = new StreamFollowerUser(streamerId, userId);
        //follower.Raise(new FollowerCreatedEvent(follower));
        return follower;
    }
}