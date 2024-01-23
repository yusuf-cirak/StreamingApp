namespace Application.Features.StreamOptions.Abstractions;

public interface IStreamOptionCommandRequest
{
    public Guid StreamerId { get; init; }
}