namespace Application.Features.Streams.Abstractions;

public interface IStreamCommandRequest
{
    public string StreamKey { get; init; }
}