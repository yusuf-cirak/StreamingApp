namespace Application.Features.StreamBlockedUsers.Abstractions;

public interface IStreamBlockedUserRequest
{
    public Guid StreamerId { get; set; }
}