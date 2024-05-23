using Application.Features.StreamFollowerUsers.Services;

namespace Application.Features.StreamFollowerUsers.Queries.GetFollowersCount;

public readonly record struct GetFollowersCountQueryRequest(Guid StreamerId) : IRequest<HttpResult<int>>;

public sealed class GetFollowersCountQueryHandler : IRequestHandler<GetFollowersCountQueryRequest, HttpResult<int>>
{
    private readonly IStreamFollowerUserService _streamFollowerUserService;

    public GetFollowersCountQueryHandler(IStreamFollowerUserService streamFollowerUserService)
    {
        _streamFollowerUserService = streamFollowerUserService;
    }

    public async Task<HttpResult<int>> Handle(GetFollowersCountQueryRequest request,
        CancellationToken cancellationToken)
        => await _streamFollowerUserService.GetStreamerFollowersCountAsync(request.StreamerId, cancellationToken);
}