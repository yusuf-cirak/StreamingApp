using Application.Common.Services;
using Application.Features.StreamFollowerUsers.Abstractions;
using Application.Features.StreamFollowerUsers.Rules;

namespace Application.Features.StreamFollowerUsers.Commands.Delete;

public readonly record struct DeleteStreamFollowerUserCommandRequest() : IStreamFollowerUserCommandRequest,
    IRequest<HttpResult>, ISecuredRequest
{
    public Guid StreamerId { get; init; }
    public Guid UserId { get; init; }
}

public sealed class
    DeleteStreamFollowerUserCommandHandler(
        StreamFollowerUserBusinessRules streamFollowerUserBusinessRules,
        ICurrentUserService currentUserService,
        IEfRepository efRepository)
    : IRequestHandler<DeleteStreamFollowerUserCommandRequest, HttpResult>
{
    public async Task<HttpResult> Handle(DeleteStreamFollowerUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        var canFollowResult =
            streamFollowerUserBusinessRules.CanUserFollowTheStreamer(request.UserId, userId);

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