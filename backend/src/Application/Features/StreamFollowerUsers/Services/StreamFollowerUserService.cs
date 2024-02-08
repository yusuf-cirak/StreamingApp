namespace Application.Features.StreamFollowerUsers.Services;

public sealed class StreamFollowerUserService : IStreamFollowerUserService
{
    private readonly IEfRepository _efRepository;

    public StreamFollowerUserService(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }


        public async Task<bool> IsUserFollowingStreamAsync(Guid streamerId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        var isBlocked = await _efRepository
            .StreamFollowerUsers
            .AnyAsync(sbu => sbu.StreamerId == streamerId && sbu.UserId == userId, cancellationToken);

        return isBlocked;
    }
}