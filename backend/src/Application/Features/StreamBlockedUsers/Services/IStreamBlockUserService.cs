using Application.Features.Streams.Dtos;

namespace Application.Features.StreamBlockedUsers.Services;

public interface IStreamBlockUserService : IDomainService<StreamBlockedUser>
{
    Task<int> BlockUserFromStreamAsync(Guid streamerId, Guid userId, CancellationToken cancellationToken = default);
    Task<int> UnblockUsersFromStreamAsync(Guid streamerId, List<Guid> userIds, CancellationToken cancellationToken = default);

    Task<bool> IsUserBlockedFromStreamAsync(Guid streamerId, Guid userId,
        CancellationToken cancellationToken);

    Task SendBlockNotificationToUsersAsync(Guid streamerId, List<Guid> userIds, bool isBlocked);

    IAsyncEnumerable<GetStreamBlockedUserDto> GetBlockedUsersOfStreamAsyncEnumerable(Guid streamerId);
}