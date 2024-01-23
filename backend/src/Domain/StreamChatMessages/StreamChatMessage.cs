namespace Domain.Entities;

public class StreamChatMessage : AuditableEntity
{
    public Guid StreamId { get; set; }
    public Guid UserId { get; set; }
    public Guid StreamerId { get; set; }
    public string Message { get; set; } = string.Empty;
    public virtual User Streamer { get; set; }
    public virtual User User { get; set; }
    public virtual Stream Stream { get; set; }

    private StreamChatMessage()
    {
    }

    private StreamChatMessage(Guid streamId, Guid userId, Guid streamerId, string message)
    {
        StreamId = streamId;
        UserId = userId;
        StreamerId = streamerId;
        Message = message;
    }

    public static StreamChatMessage Create(Guid streamId, Guid userId, Guid streamerId, string message)
    {
        StreamChatMessage streamChatMessage = new(streamId, userId, streamerId, message);
        // streamChatMessage.Raise(new StreamChatMessageCreatedEvent(streamChatMessage));
        return streamChatMessage;
    }
}