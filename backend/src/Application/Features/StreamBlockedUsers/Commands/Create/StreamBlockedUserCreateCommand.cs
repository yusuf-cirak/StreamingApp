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
    private readonly StreamBlockedUserBusinessRules _streamBlockedUserBusinessRules;
    private readonly IStreamBlockUserService _streamBlockUserService;

    public CreateStreamBlockedUserCreateHandler(
        StreamBlockedUserBusinessRules streamBlockedUserBusinessRules, IStreamBlockUserService streamBlockUserService)
    {
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



        _ = Task.Run(() => _streamBlockUserService.SendBlockNotificationToUsersAsync(request.StreamerId, [request.BlockedUserId],
            isBlocked: true), cancellationToken);


        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : StreamBlockedUserErrors.FailedToBlockUser;
    }
}