using System.Security.Claims;
using Application.Features.StreamBlockedUsers.Abstractions;
using Application.Features.StreamBlockedUsers.Rules;

namespace Application.Features.StreamBlockedUsers.Commands.Delete;

public readonly record struct StreamBlockedUserDeleteCommandRequest
    : IStreamBlockedUserRequest, IRequest<HttpResult>, ISecuredRequest
{
    public Guid StreamerId { get; init; }

    public Guid BlockedUserId { get; init; }
    public List<Func<ICollection<Claim>, object, Result>> AuthorizationRules { get; }

    public StreamBlockedUserDeleteCommandRequest()
    {
        AuthorizationRules = [StreamBlockedUserAuthorizationRules.CanUserBlockOrUnblockAUserFromStream];
    }

    public StreamBlockedUserDeleteCommandRequest(Guid streamerId, Guid blockedUserId) : this()
    {
        StreamerId = streamerId;
        BlockedUserId = blockedUserId;
    }
}

public sealed class
    StreamBlockedUserDeleteCommandHandler : IRequestHandler<StreamBlockedUserDeleteCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;

    public StreamBlockedUserDeleteCommandHandler(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<HttpResult> Handle(StreamBlockedUserDeleteCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _efRepository
            .StreamBlockedUsers
            .Where(sbu => sbu.UserId == request.BlockedUserId && sbu.StreamerId == request.StreamerId)
            .ExecuteDeleteAsync(cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : StreamBlockedUserErrors.FailedToUnblockUser;
    }
}