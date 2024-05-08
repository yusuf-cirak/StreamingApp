using SignalR.Hubs.Stream.Client.Abstractions.Services;
using SignalR.Models;

namespace Application.Features.Streams.Queries.GetViewers;

public sealed record GetStreamViewersQueryRequest(string StreamerName) : IRequest<HttpResult<IEnumerable<HubUserDto>>>;

public sealed class
    GetStreamViewersQueryRequestHandler : IRequestHandler<GetStreamViewersQueryRequest,
    HttpResult<IEnumerable<HubUserDto>>>
{
    private readonly IStreamHubClientService _hubClientService;

    public GetStreamViewersQueryRequestHandler(IStreamHubClientService hubClientService)
    {
        _hubClientService = hubClientService;
    }

    public async Task<HttpResult<IEnumerable<HubUserDto>>> Handle(GetStreamViewersQueryRequest request,
        CancellationToken cancellationToken)
    {
        var streamViewers = await _hubClientService.GetStreamViewersAsync(request.StreamerName);

        return HttpResult<IEnumerable<HubUserDto>>.Success(streamViewers);
    }
}