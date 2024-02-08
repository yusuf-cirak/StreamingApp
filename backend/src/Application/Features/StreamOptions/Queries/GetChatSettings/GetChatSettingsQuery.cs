using Application.Common.Mapping;
using Application.Features.StreamOptions.Dtos;
using Application.Features.StreamOptions.Rules;

namespace Application.Features.StreamOptions.Queries.GetChatSettings;

public readonly record struct GetStreamChatSettingsQueryRequest : IRequest<HttpResult<GetStreamChatSettingsDto>>,
    ISecuredRequest
{
    public Guid StreamerId { get; init; }

    public AuthorizationFunctions AuthorizationFunctions { get; }

    public GetStreamChatSettingsQueryRequest()
    {
        AuthorizationFunctions = [StreamOptionAuthorizationRules.CanUserGetOrUpdateStreamOptions];
    }
}

public sealed class
    GetStreamChatSettingsQueryHandler : IRequestHandler<GetStreamChatSettingsQueryRequest,
    HttpResult<GetStreamChatSettingsDto>>
{
    private readonly IEfRepository _efRepository;

    public GetStreamChatSettingsQueryHandler(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<HttpResult<GetStreamChatSettingsDto>> Handle(GetStreamChatSettingsQueryRequest request,
        CancellationToken cancellationToken)
    {
        return await _efRepository
            .StreamOptions
            .Where(so => so.Id == request.StreamerId)
            .Select(so => so.ToStreamChatSettingsDto())
            .SingleOrDefaultAsync(cancellationToken);
    }
}