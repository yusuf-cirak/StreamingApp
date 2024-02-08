namespace Application.Features.StreamBlockedUsers.Services;

public interface IStreamBlockedUserService : IDomainService<StreamBlockedUser>
{
    Task<bool> IsUserBlockedFromStreamAsync(Guid streamerId, Guid userId,
        CancellationToken cancellationToken);
}