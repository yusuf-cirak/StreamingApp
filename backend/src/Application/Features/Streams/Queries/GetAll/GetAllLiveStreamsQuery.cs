using Application.Common.Mapping;
using Application.Features.Streams.Services;

namespace Application.Features.Streams.Queries.GetAll;

public readonly record struct GetAllLiveStreamsQueryRequest() : IRequest<HttpResult<List<GetStreamDto>>>;

public sealed class
    GetAllLiveStreamsQueryHandler : IRequestHandler<GetAllLiveStreamsQueryRequest, HttpResult<List<GetStreamDto>>>
{
    private readonly IStreamCacheService _streamCacheService;
    private readonly IEfRepository _efRepository;

    public GetAllLiveStreamsQueryHandler(IStreamCacheService streamCacheService, IEfRepository efRepository)
    {
        _streamCacheService = streamCacheService;
        _efRepository = efRepository;
    }

    public Task<HttpResult<List<GetStreamDto>>> Handle(GetAllLiveStreamsQueryRequest request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<HttpResult<List<GetStreamDto>>>(_streamCacheService.LiveStreamers);
    }
}