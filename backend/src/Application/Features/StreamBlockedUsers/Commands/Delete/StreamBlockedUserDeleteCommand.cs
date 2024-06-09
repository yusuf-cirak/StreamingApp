using Application.Common.Permissions;
using Application.Features.StreamBlockedUsers.Abstractions;
using Application.Features.StreamBlockedUsers.Services;

namespace Application.Features.StreamBlockedUsers.Commands.Delete;

public record struct StreamBlockedUserDeleteCommandRequest
    : IStreamBlockedUserRequest, IRequest<HttpResult>, IPermissionRequest
{
    private Guid _streamerId;

    public Guid StreamerId
    {
        get => _streamerId;
        set
        {
            _streamerId = value;
            this.SetPermissionRequirements();
        }
    }


    public List<Guid> BlockedUserIds { get; init; }

    public PermissionRequirements PermissionRequirements { get; private set; }

    private void SetPermissionRequirements()
    {
        PermissionRequirements = PermissionRequirements
            .Create()
            .WithRequiredValue(this.StreamerId.ToString())
            .WithRoles(PermissionHelper.AllStreamRoles().ToArray())
            .WithOperationClaims(RequiredClaim.Create(OperationClaimConstants.Stream.Write.BlockFromChat,
                StreamErrors.UserIsNotModeratorOfStream))
            .WithNameIdentifierClaim();
    }
}

public sealed class
    StreamBlockedUserDeleteCommandHandler(IStreamBlockUserService streamBlockUserService)
    : IRequestHandler<StreamBlockedUserDeleteCommandRequest, HttpResult>
{
    public async Task<HttpResult> Handle(StreamBlockedUserDeleteCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result =
            await streamBlockUserService.UnblockUsersFromStreamAsync(request.StreamerId, request.BlockedUserIds,
                cancellationToken);

        _ = Task.Run(() => streamBlockUserService.SendBlockNotificationToUsersAsync(request.StreamerId,
            request.BlockedUserIds,
            isBlocked: false));

        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : StreamBlockedUserErrors.FailedToUnblockUser;
    }
}