using Application.Common.Extensions;
using Application.Features.StreamBlockedUsers.Services;

namespace Application.Features.StreamBlockedUsers.Queries.GetIsBlockedFromStream;
public readonly record struct GetIsUserBlockedFromStreamQueryRequest(Guid StreamerId) : ISecuredRequest,IRequest<Boolean>
{
    public AuthorizationFunctions AuthorizationFunctions => [];
}


public sealed class GetIsUserBlockedFromStreamQueryRequestHandler : IRequestHandler<GetIsUserBlockedFromStreamQueryRequest, Boolean>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStreamBlockedUserService _streamBlockedUserService;

    public GetIsUserBlockedFromStreamQueryRequestHandler(IHttpContextAccessor httpContextAccessor, IStreamBlockedUserService streamBlockedUserService)
    {
        _httpContextAccessor = httpContextAccessor;
        _streamBlockedUserService = streamBlockedUserService;
    }

    public async Task<bool> Handle(GetIsUserBlockedFromStreamQueryRequest request, CancellationToken cancellationToken)
    {
        _ = Guid.TryParse(_httpContextAccessor.HttpContext.User.GetUserId(), out Guid userId);

       return await _streamBlockedUserService.IsUserBlockedFromStreamAsync(request.StreamerId, userId, cancellationToken);
    }
}