namespace Application.Features.StreamFollowerUsers.Services;

public sealed class StreamFollowerUserService : IStreamFollowerUserService
{
    private readonly IEfRepository _efRepository;

    public StreamFollowerUserService(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }


    public Task<bool> IsUserFollowingStreamAsync(Guid streamerId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        return _efRepository
            .StreamFollowerUsers
            .AnyAsync(sbu => sbu.StreamerId == streamerId && sbu.UserId == userId, cancellationToken);
    }

    public Task<int> GetStreamerFollowersCountAsync(Guid streamerId, CancellationToken cancellationToken = default)
    {
        return _efRepository
            .StreamFollowerUsers
            .Where(sfu => sfu.StreamerId == streamerId)
            .CountAsync(cancellationToken);
    }
}