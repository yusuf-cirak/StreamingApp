namespace Application.Features.Users.Services;

public interface IUserService : IDomainService<User>
{
    Task<List<GetStreamDto>> GetFollowingStreamsAsync(Guid userId,
        CancellationToken cancellationToken = default);

    IEnumerable<GetBlockedStreamDto> GetBlockedStreamsEnumerable(Guid currentUserId);
}