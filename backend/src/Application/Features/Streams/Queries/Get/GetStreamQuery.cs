using Application.Contracts.Streams;
using Application.Features.Streams.Services;

namespace Application.Features.Streams.Queries.Get;

public readonly record struct GetStreamQueryRequest(string StreamerName) : IRequest<HttpResult<GetStreamDto>>;

public sealed class
    GetStreamQueryHandler : IRequestHandler<GetStreamQueryRequest, HttpResult<GetStreamDto>>
{
    private readonly IStreamService _streamService;

    public GetStreamQueryHandler(IStreamService streamService)
    {
        _streamService = streamService;
    }

    public async Task<HttpResult<GetStreamDto>> Handle(GetStreamQueryRequest request,
        CancellationToken cancellationToken)
    {
        var liveStreamResult = await _streamService.GetLiveStreamerByNameAsync(request.StreamerName);

        return liveStreamResult
            .Match<HttpResult<GetStreamDto>>
            (streamer => streamer,
                failure => failure);
    }
}