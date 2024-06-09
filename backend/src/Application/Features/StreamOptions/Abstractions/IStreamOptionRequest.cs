namespace Application.Features.StreamOptions.Abstractions;

public interface IStreamOptionRequest
{
    public Guid StreamerId { get; set; }
}
