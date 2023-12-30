namespace Application.Features.Streamers.Abstractions;

public interface IStreamerCommandRequest
{
    public Guid StreamerId { get; init; }
}