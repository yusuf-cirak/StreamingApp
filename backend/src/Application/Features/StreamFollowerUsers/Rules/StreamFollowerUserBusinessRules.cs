using Application.Abstractions;

namespace Application.Features.StreamFollowerUsers.Rules;

public sealed class StreamFollowerUserBusinessRules : BaseBusinessRules
{
    private readonly IEfRepository _efRepository;

    public StreamFollowerUserBusinessRules(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }
        public async Task<bool> IsUserFollowingTheStreamAsync(Guid streamerId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        var isBlocked = await _efRepository
            .StreamFollowerUsers
            .AnyAsync(sbu => sbu.StreamerId == streamerId && sbu.UserId == userId, cancellationToken);

        return isBlocked;
    }
}