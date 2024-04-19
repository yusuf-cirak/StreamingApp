using Application.Features.Streams.Services;

namespace Application.Features.Streams.Queries.GetAll;

public readonly record struct GetAllLiveStreamsQueryRequest() : IRequest<HttpResult<List<GetStreamDto>>>;

public sealed class
    GetAllLiveStreamsQueryHandler : IRequestHandler<GetAllLiveStreamsQueryRequest, HttpResult<List<GetStreamDto>>>
{
    private readonly IStreamService _streamService;

    public GetAllLiveStreamsQueryHandler(IStreamService streamService)
    {
        _streamService = streamService;
    }

    public async Task<HttpResult<List<GetStreamDto>>> Handle(GetAllLiveStreamsQueryRequest request,
        CancellationToken cancellationToken)
    {
        return await _streamService.GetLiveStreamsAsync(cancellationToken);
    }
}