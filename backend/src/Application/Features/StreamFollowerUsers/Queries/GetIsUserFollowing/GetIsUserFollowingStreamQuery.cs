using Application.Common.Permissions;
using Application.Features.StreamFollowerUsers.Services;

namespace Application.Features.StreamFollowerUsers.Queries.GetIsUserFollowing;

public readonly record struct GetIsUserFollowingStreamQueryRequest(Guid StreamerId)
    : ISecuredRequest, IRequest<HttpResult<bool>>;

public sealed class
    GetIsUserBlockedFromStreamQueryRequestHandler(
        IHttpContextAccessor httpContextAccessor,
        IStreamFollowerUserService streamFollowerUserService)
    : IRequestHandler<GetIsUserFollowingStreamQueryRequest,
        HttpResult<bool>>
{
    public async Task<HttpResult<bool>> Handle(GetIsUserFollowingStreamQueryRequest request,
        CancellationToken cancellationToken)
    {
        _ = Guid.TryParse(httpContextAccessor.HttpContext.User.GetUserId(), out Guid userId);

        return HttpResult<bool>.Success(
            await streamFollowerUserService.IsUserFollowingStreamAsync(request.StreamerId, userId, cancellationToken));
    }
}