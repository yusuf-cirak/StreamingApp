using Application.Common.Extensions;
using Application.Features.StreamOptions.Abstractions;
using Application.Features.StreamOptions.Rules;

namespace Application.Features.StreamOptions.Queries.GetStreamKey;

public readonly record struct GetStreamKeyQueryRequest : IRequest<HttpResult<string>>, ISecuredRequest , IStreamOptionRequest
{
    public Guid StreamerId { get; init; }
    public AuthorizationFunctions AuthorizationFunctions { get; }


    public GetStreamKeyQueryRequest()
    {
        AuthorizationFunctions = [StreamOptionAuthorizationRules.UserMustBeStreamer];
    }



    public GetStreamKeyQueryRequest(Guid streamerId) : this()
    {
        StreamerId = streamerId;
    }

}

public sealed class GetStreamKeyQueryHandler : IRequestHandler<GetStreamKeyQueryRequest, HttpResult<string>>
{
    private readonly IEfRepository _efRepository;

    public GetStreamKeyQueryHandler(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<HttpResult<string>> Handle(GetStreamKeyQueryRequest request, CancellationToken cancellationToken)
    {

        return await _efRepository
            .StreamOptions
            .Where(r => r.Id == request.StreamerId)
            .Select(r => r.StreamKey)
            .SingleOrDefaultAsync(cancellationToken);
    }
}