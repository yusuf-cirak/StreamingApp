namespace Application.Features.StreamFollowerUsers.Abstractions;

public interface IStreamFollowerUserCommandRequest
{
    public Guid StreamerId { get; init; }
    public Guid UserId { get; init; }

}