using Application.Common.Permissions;
using Application.Common.Services;
using Application.Features.StreamFollowerUsers.Abstractions;
using Application.Features.StreamFollowerUsers.Rules;

namespace Application.Features.StreamFollowerUsers.Commands.Delete;

public readonly record struct DeleteStreamFollowerUserCommandRequest() : IStreamFollowerUserCommandRequest,
    IRequest<HttpResult>, ISecuredRequest
{
    public Guid StreamerId { get; init; }
    public Guid UserId { get; init; }

    public PermissionRequirements PermissionRequirements { get; } = PermissionRequirements.Create();
}

public sealed class
    DeleteStreamFollowerUserCommandHandler(
        StreamFollowerUserBusinessRules streamFollowerUserBusinessRules,
        CurrentUserService currentUserService,
        IEfRepository efRepository)
    : IRequestHandler<DeleteStreamFollowerUserCommandRequest, HttpResult>
{
    public async Task<HttpResult> Handle(DeleteStreamFollowerUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        var canFollowResult =
            streamFollowerUserBusinessRules.CanUserFollowTheStreamer(request.UserId, currentUserService.UserId);

        if (canFollowResult.IsFailure)
        {
            return canFollowResult.Error;
        }
        

        var result = await efRepository
            .StreamFollowerUsers
            .Where(sfu => sfu.UserId == request.UserId && sfu.StreamerId == request.StreamerId)
            .ExecuteDeleteAsync(cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : HttpResult.Failure(StreamFollowerUserErrors.FailedToRemoveFollowerUserFromStream,
                StatusCodes.Status400BadRequest);
    }
}