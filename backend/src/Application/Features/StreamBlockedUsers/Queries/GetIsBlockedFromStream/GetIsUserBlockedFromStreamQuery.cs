using Application.Features.StreamBlockedUsers.Services;

namespace Application.Features.StreamBlockedUsers.Queries.GetIsBlockedFromStream;

public readonly record struct GetIsUserBlockedFromStreamQueryRequest(Guid StreamerId)
    : ISecuredRequest, IRequest<bool>;

public sealed class
    GetIsUserBlockedFromStreamQueryRequestHandler : IRequestHandler<GetIsUserBlockedFromStreamQueryRequest, Boolean>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStreamBlockUserService _streamBlockUserService;

    public GetIsUserBlockedFromStreamQueryRequestHandler(IHttpContextAccessor httpContextAccessor,
        IStreamBlockUserService streamBlockUserService)
    {
        _httpContextAccessor = httpContextAccessor;
        _streamBlockUserService = streamBlockUserService;
    }

    public async Task<bool> Handle(GetIsUserBlockedFromStreamQueryRequest request, CancellationToken cancellationToken)
    {
        _ = Guid.TryParse(_httpContextAccessor.HttpContext.User.GetUserId(), out Guid userId);

        return await _streamBlockUserService.IsUserBlockedFromStreamAsync(request.StreamerId, userId,
            cancellationToken);
    }
}