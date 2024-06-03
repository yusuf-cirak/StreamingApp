using Application.Common.Permissions;
using Application.Features.Streams.Services;

namespace Application.Features.Streams.Queries.GetModerating;

public readonly record struct GetModeratingStreamsQueryRequest()
    : IRequest<HttpResult<List<GetUserDto>>>, ISecuredRequest
{
    public PermissionRequirements PermissionRequirements { get; } = PermissionRequirements.Create();
}

public sealed class
    GetModeratingStreamsQueryHandler : IRequestHandler<GetModeratingStreamsQueryRequest, HttpResult<List<GetUserDto>>>
{
    private readonly IStreamService _streamService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetModeratingStreamsQueryHandler(IStreamService streamService, IHttpContextAccessor httpContextAccessor)
    {
        _streamService = streamService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HttpResult<List<GetUserDto>>> Handle(GetModeratingStreamsQueryRequest request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor.HttpContext!.User.GetUserId());
        return await _streamService.GetModeratingStreamsAsync(userId, cancellationToken);
    }
}