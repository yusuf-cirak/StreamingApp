using Application.Common.Services;
using Application.Features.StreamFollowerUsers.Abstractions;
using Application.Features.StreamFollowerUsers.Rules;

namespace Application.Features.StreamFollowerUsers.Commands.Create;

public readonly record struct StreamFollowerUserCreateCommandRequest() : IStreamFollowerUserCommandRequest,
    IRequest<HttpResult>,
    ISecuredRequest
{
    public Guid StreamerId { get; init; }
    public Guid UserId { get; init; }
}

public sealed class
    StreamFollowerUserCreateCommandHandler(
        ICurrentUserService currentUserService,
        IEfRepository efRepository,
        StreamFollowerUserBusinessRules streamFollowerUserBusinessRules)
    : IRequestHandler<StreamFollowerUserCreateCommandRequest, HttpResult>
{
    public async Task<HttpResult> Handle(StreamFollowerUserCreateCommandRequest request,
        CancellationToken cancellationToken)
    {
        var canFollowResult =
            streamFollowerUserBusinessRules.CanUserFollowTheStreamer(request.UserId, currentUserService.UserId);

        if (canFollowResult.IsFailure)
        {
            return canFollowResult.Error;
        }

        var streamFollowerUser = StreamFollowerUser.Create(request.StreamerId, request.UserId);

        efRepository.StreamFollowerUsers.Add(streamFollowerUser);

        var result = await efRepository.SaveChangesAsync(cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status201Created)
            : StreamFollowerUserErrors.FailedToFollowStreamer;
    }
}