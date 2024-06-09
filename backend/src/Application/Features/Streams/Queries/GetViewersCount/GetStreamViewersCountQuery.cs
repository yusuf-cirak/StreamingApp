using SignalR.Hubs.Stream.Client.Abstractions.Services;

namespace Application.Features.Streams.Queries.GetViewersCount;

public sealed record GetStreamViewersCountQueryRequest(string StreamerName)
    : IRequest<HttpResult<int>>;

public sealed class
    GetStreamViewersQueryRequestHandler : IRequestHandler<GetStreamViewersCountQueryRequest,
    HttpResult<int>>
{
    private readonly IStreamHubClientService _hubClientService;

    public GetStreamViewersQueryRequestHandler(IStreamHubClientService hubClientService)
    {
        _hubClientService = hubClientService;
    }

    public async Task<HttpResult<int>> Handle(GetStreamViewersCountQueryRequest request,
        CancellationToken cancellationToken)
    {
        return await _hubClientService.GetStreamViewersCountAsync(request.StreamerName);
    }
}