using Application.Common.Mapping;
using Application.Features.StreamOptions.Dtos;
using Application.Features.StreamOptions.Rules;

namespace Application.Features.StreamOptions.Queries.GetStreamTitleDescription;

public readonly record struct
    GetStreamTitleDescriptionQueryRequest : IRequest<HttpResult<GetStreamTitleDescriptionDto>>, ISecuredRequest
{
    public Guid StreamerId { get; init; }
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public GetStreamTitleDescriptionQueryRequest()
    {
        AuthorizationFunctions = [StreamOptionAuthorizationRules.CanUserGetOrUpdateStreamOptions];
    }
}

public sealed class GetStreamTitleDescriptionQueryHandler : IRequestHandler<GetStreamTitleDescriptionQueryRequest,
    HttpResult<GetStreamTitleDescriptionDto>>
{
    private readonly IEfRepository _efRepository;

    public GetStreamTitleDescriptionQueryHandler(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<HttpResult<GetStreamTitleDescriptionDto>> Handle(GetStreamTitleDescriptionQueryRequest request,
        CancellationToken cancellationToken)
    {
        return await _efRepository
            .StreamOptions
            .Where(so => so.Id == request.StreamerId)
            .Select(so => so.ToStreamTitleDescriptionDto())
            .SingleOrDefaultAsync(cancellationToken);
    }
}