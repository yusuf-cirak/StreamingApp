using Application.Abstractions;

namespace Application.Features.StreamBlockedUsers.Rules;

public sealed class StreamBlockedUserBusinessRules : BaseBusinessRules
{
    private readonly IEfRepository _efRepository;

    public StreamBlockedUserBusinessRules(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<Result> BlockedUserIsNotAlreadyBlocked(Guid streamerId, Guid blockedUserId,
        CancellationToken cancellationToken = default)
    {
        var isBlocked = await _efRepository
            .StreamBlockedUsers
            .AnyAsync(sbu => sbu.StreamerId == streamerId && sbu.UserId == blockedUserId, cancellationToken);

        return isBlocked
            ? Result.Failure(StreamBlockedUserErrors.UserIsAlreadyBlocked)
            : Result.Success();
    }
}