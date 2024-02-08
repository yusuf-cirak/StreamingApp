using Application.Common.Extensions;
using Application.Features.StreamFollowerUsers.Services;

namespace Application.Features.StreamFollowerUsers.Queries.GetIsUserFollowingStream;
public readonly record struct GetIsUserFollowingStreamQueryRequest(Guid StreamerId) : ISecuredRequest,IRequest<Boolean>
{
    public AuthorizationFunctions AuthorizationFunctions => [];
}


public sealed class GetIsUserBlockedFromStreamQueryRequestHandler : IRequestHandler<GetIsUserFollowingStreamQueryRequest, Boolean>
{
    private readonly IStreamFollowerUserService _streamFollowerUserService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetIsUserBlockedFromStreamQueryRequestHandler(IHttpContextAccessor httpContextAccessor, IStreamFollowerUserService streamFollowerUserService)
    {
        _httpContextAccessor = httpContextAccessor;
        _streamFollowerUserService = streamFollowerUserService;
    }

    public async Task<bool> Handle(GetIsUserFollowingStreamQueryRequest request, CancellationToken cancellationToken)
    {
        _ = Guid.TryParse(_httpContextAccessor.HttpContext.User.GetUserId(), out Guid userId);

       return await _streamFollowerUserService.IsUserFollowingStreamAsync(request.StreamerId, userId, cancellationToken);
    }
}