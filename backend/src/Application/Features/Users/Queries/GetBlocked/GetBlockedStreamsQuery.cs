using Application.Features.Users.Services;

namespace Application.Features.Users.Queries.GetBlocked;

public sealed record GetBlockedStreamsQueryRequest : IRequest<HttpResult<IEnumerable<GetBlockedStreamDto>>>,
    ISecuredRequest
{
    public AuthorizationFunctions AuthorizationFunctions { get; } = new();
}

public sealed class GetBlockedStreamsQueryRequestHandler : IRequestHandler<GetBlockedStreamsQueryRequest,
    HttpResult<IEnumerable<GetBlockedStreamDto>>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;

    public GetBlockedStreamsQueryRequestHandler(IHttpContextAccessor httpContextAccessor, IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
    }

    public Task<HttpResult<IEnumerable<GetBlockedStreamDto>>> Handle(GetBlockedStreamsQueryRequest request,
        CancellationToken cancellationToken)
    {
        Guid.TryParse(_httpContextAccessor.HttpContext.User.GetUserId(), out Guid currentUserId);

        var blockedStreams = _userService.GetBlockedStreamsEnumerable(currentUserId);

        var result = Task.FromResult(HttpResult<IEnumerable<GetBlockedStreamDto>>.Success(blockedStreams));

        return result;
    }
}