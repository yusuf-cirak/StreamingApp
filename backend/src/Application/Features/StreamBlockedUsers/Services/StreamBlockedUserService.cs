namespace Application.Features.StreamBlockedUsers.Services;

public sealed class StreamBlockedUserService : IStreamBlockedUserService
{
    private readonly IEfRepository _efRepository;

    public StreamBlockedUserService(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }


        public async Task<bool> IsUserBlockedFromStreamAsync(Guid streamerId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        var isBlocked = await _efRepository
            .StreamBlockedUsers
            .AnyAsync(sbu => sbu.StreamerId == streamerId && sbu.UserId == userId, cancellationToken);

        return isBlocked;
    }
}