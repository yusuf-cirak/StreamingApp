using Application.Features.Users.Services;

namespace Application.Features.Streams.Queries.GetFollowing;

public readonly record struct GetFollowingStreamsQueryRequest : IRequest<HttpResult<List<GetFollowingStreamDto>>>,
    ISecuredRequest
{
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public GetFollowingStreamsQueryRequest()
    {
        AuthorizationFunctions = [];
    }
}

public sealed class
    GetFollowingStreamsQueryHandler : IRequestHandler<GetFollowingStreamsQueryRequest, HttpResult<List<GetFollowingStreamDto>>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;

    public GetFollowingStreamsQueryHandler(IHttpContextAccessor httpContextAccessor, IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
    }

    public async Task<HttpResult<List<GetFollowingStreamDto>>> Handle(GetFollowingStreamsQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.GetUserId());

        return await _userService.GetFollowingStreamsAsync(userId, cancellationToken);
    }
}