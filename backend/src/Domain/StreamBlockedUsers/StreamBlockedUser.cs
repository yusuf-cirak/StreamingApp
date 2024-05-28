namespace Domain.Entities;

public class StreamBlockedUser : AuditableEntity
{
    public Guid StreamerId { get; set; }
    public Guid UserId { get; set; }
    public virtual User Streamer { get; set; }
    public virtual User User { get; set; }

    private StreamBlockedUser()
    {
    }

    private StreamBlockedUser(Guid streamerId, Guid userId)
    {
        StreamerId = streamerId;
        UserId = userId;
    }

    public static StreamBlockedUser Create(Guid streamerId, Guid userId)
    {
        var streamBlockedUser = new StreamBlockedUser(streamerId, userId);
        //follower.Raise(new FollowerCreatedEvent(follower));
        return streamBlockedUser;
    }
}