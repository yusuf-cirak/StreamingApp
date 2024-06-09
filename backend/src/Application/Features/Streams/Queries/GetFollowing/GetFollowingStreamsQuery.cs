using Application.Common.Permissions;
using Application.Features.Users.Services;

namespace Application.Features.Streams.Queries.GetFollowing;

public readonly record struct GetFollowingStreamsQueryRequest() : IRequest<HttpResult<List<GetStreamDto>>>,
    ISecuredRequest;

public sealed class
    GetFollowingStreamsQueryHandler(IHttpContextAccessor httpContextAccessor, IUserService userService)
    : IRequestHandler<GetFollowingStreamsQueryRequest, HttpResult<List<GetStreamDto>>>
{
    public async Task<HttpResult<List<GetStreamDto>>> Handle(GetFollowingStreamsQueryRequest request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(httpContextAccessor.HttpContext.User.GetUserId());

        return await userService.GetFollowingStreamsAsync(userId, cancellationToken);
    }
}