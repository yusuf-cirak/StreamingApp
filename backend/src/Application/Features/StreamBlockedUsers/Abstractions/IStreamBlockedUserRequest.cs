namespace Application.Features.StreamBlockedUsers.Abstractions;

public interface IStreamBlockedUserRequest
{
    public Guid StreamerId { get; init; }
    public Guid BlockedUserId { get; init; }
}