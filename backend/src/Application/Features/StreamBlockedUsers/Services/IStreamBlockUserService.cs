namespace Application.Features.StreamBlockedUsers.Services;

public interface IStreamBlockUserService : IDomainService<StreamBlockedUser>
{
    Task<int> BlockUserFromStreamAsync(Guid streamerId, Guid userId, CancellationToken cancellationToken = default);
    Task<int> UnblockUserFromStreamAsync(Guid streamerId, Guid userId, CancellationToken cancellationToken = default);

    Task<bool> IsUserBlockedFromStreamAsync(Guid streamerId, Guid userId,
        CancellationToken cancellationToken);

    Task SendBlockNotificationToUserAsync(Guid streamerId, Guid userId, bool isBlocked);
}