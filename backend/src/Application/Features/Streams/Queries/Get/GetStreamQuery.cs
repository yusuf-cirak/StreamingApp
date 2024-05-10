using Application.Features.Streams.Services;

namespace Application.Features.Streams.Queries.Get;

public readonly record struct GetStreamQueryRequest(string StreamerName) : IRequest<HttpResult<GetStreamInfoDto>>;

public sealed class
    GetStreamQueryHandler : IRequestHandler<GetStreamQueryRequest, HttpResult<GetStreamInfoDto>>
{
    private readonly IStreamService _streamService;

    public GetStreamQueryHandler(IStreamService streamService)
    {
        _streamService = streamService;
    }

    public async Task<HttpResult<GetStreamInfoDto>> Handle(GetStreamQueryRequest request,
        CancellationToken cancellationToken)
        => await _streamService.GetLiveStreamerByNameAsync(request.StreamerName, cancellationToken);
}