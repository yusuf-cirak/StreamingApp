namespace Application.Features.Streams.Abstractions;

public interface IStreamRequest
{
    public Guid StreamerId { get; init; }
}