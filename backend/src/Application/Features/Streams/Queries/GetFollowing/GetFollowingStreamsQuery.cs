using Application.Common.Extensions;
using Application.Contracts.Streams;
using Application.Features.Streams.Services;

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
    private readonly IStreamService _streamService;

    public GetFollowingStreamsQueryHandler(IHttpContextAccessor httpContextAccessor, IStreamService streamService)
    {
        _httpContextAccessor = httpContextAccessor;
        _streamService = streamService;
    }

    public async Task<HttpResult<List<GetFollowingStreamDto>>> Handle(GetFollowingStreamsQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.GetUserId());

        return await _streamService.GetFollowingStreamsAsync(userId, cancellationToken);
    }
}