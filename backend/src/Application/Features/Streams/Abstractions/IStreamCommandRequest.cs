namespace Application.Features.Streams.Abstractions;

public interface IStreamCommandRequest
{
    public Guid StreamerId { get; init; }
}