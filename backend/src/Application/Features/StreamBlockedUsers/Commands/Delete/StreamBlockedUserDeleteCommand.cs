using Application.Features.StreamBlockedUsers.Abstractions;
using Application.Features.StreamBlockedUsers.Rules;
using Application.Features.StreamBlockedUsers.Services;

namespace Application.Features.StreamBlockedUsers.Commands.Delete;

public readonly record struct StreamBlockedUserDeleteCommandRequest
    : IStreamBlockedUserRequest, IRequest<HttpResult>, ISecuredRequest
{
    public Guid StreamerId { get; init; }

    public Guid BlockedUserId { get; init; }
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public StreamBlockedUserDeleteCommandRequest()
    {
        AuthorizationFunctions = [StreamBlockedUserAuthorizationRules.CanUserBlockOrUnblockAUserFromStream];
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
    private readonly IStreamBlockUserService _streamBlockUserService;

    public StreamBlockedUserDeleteCommandHandler(IEfRepository efRepository,
        IStreamBlockUserService streamBlockUserService)
    {
        _efRepository = efRepository;
        _streamBlockUserService = streamBlockUserService;
    }

    public async Task<HttpResult> Handle(StreamBlockedUserDeleteCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result =
            await _streamBlockUserService.UnblockUserFromStreamAsync(request.StreamerId, request.BlockedUserId,
                cancellationToken);

        _ = _streamBlockUserService.SendBlockNotificationToUserAsync(request.StreamerId, request.BlockedUserId,
            isBlocked: false);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : StreamBlockedUserErrors.FailedToUnblockUser;
    }
}