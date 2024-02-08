using Application.Common.Extensions;
using Application.Features.StreamOptions.Rules;

namespace Application.Features.StreamOptions.Queries.GetStreamKey;

public readonly record struct GetStreamKeyQueryRequest : IRequest<HttpResult<string>>, ISecuredRequest
{
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public GetStreamKeyQueryRequest()
    {
        AuthorizationFunctions = [StreamOptionAuthorizationRules.UserMustBeStreamer];
    }
}

public sealed class GetStreamKeyQueryHandler : IRequestHandler<GetStreamKeyQueryRequest, HttpResult<string>>
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetStreamKeyQueryHandler(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HttpResult<string>> Handle(GetStreamKeyQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor!.HttpContext!.User.GetUserId());

        return await _efRepository
            .StreamOptions
            .Where(r => r.Id == userId)
            .Select(r => r.StreamKey)
            .SingleOrDefaultAsync(cancellationToken);
    }
}