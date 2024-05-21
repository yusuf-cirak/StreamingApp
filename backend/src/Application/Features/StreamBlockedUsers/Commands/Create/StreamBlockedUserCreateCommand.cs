using Application.Features.StreamBlockedUsers.Abstractions;
using Application.Features.StreamBlockedUsers.Rules;
using Application.Features.StreamBlockedUsers.Services;

namespace Application.Features.StreamBlockedUsers.Commands.Create;

public readonly record struct CreateStreamBlockedUserCreateCommandRequest
    : IStreamBlockedUserRequest, IRequest<HttpResult>, ISecuredRequest
{
    public Guid StreamerId { get; init; }

    public Guid BlockedUserId { get; init; }
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public CreateStreamBlockedUserCreateCommandRequest()
    {
        AuthorizationFunctions = [StreamBlockedUserAuthorizationRules.CanUserBlockOrUnblockAUserFromStream];
    }

    public CreateStreamBlockedUserCreateCommandRequest(Guid streamerId, Guid blockedUserId) : this()
    {
        StreamerId = streamerId;
        BlockedUserId = blockedUserId;
    }
}

public sealed class
    CreateStreamBlockedUserCreateHandler : IRequestHandler<CreateStreamBlockedUserCreateCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly StreamBlockedUserBusinessRules _streamBlockedUserBusinessRules;
    private readonly IStreamBlockUserService _streamBlockUserService;

    public CreateStreamBlockedUserCreateHandler(IEfRepository efRepository,
        StreamBlockedUserBusinessRules streamBlockedUserBusinessRules, IStreamBlockUserService streamBlockUserService)
    {
        _efRepository = efRepository;
        _streamBlockedUserBusinessRules = streamBlockedUserBusinessRules;
        _streamBlockUserService = streamBlockUserService;
    }

    public async Task<HttpResult> Handle(CreateStreamBlockedUserCreateCommandRequest request,
        CancellationToken cancellationToken)
    {
        var streamBlockedUserExistResult =
            await _streamBlockedUserBusinessRules.BlockedUserIsNotAlreadyBlockedAsync(request.StreamerId,
                request.BlockedUserId, cancellationToken);

        if (streamBlockedUserExistResult.IsFailure)
        {
            return streamBlockedUserExistResult.Error;
        }

        var result = await _streamBlockUserService.BlockUserFromStreamAsync(request.StreamerId, request.BlockedUserId,
            cancellationToken);


        _ = _streamBlockUserService.SendBlockNotificationToUserAsync(request.StreamerId, request.BlockedUserId,
            isBlocked: true);


        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : StreamBlockedUserErrors.FailedToBlockUser;
    }
}