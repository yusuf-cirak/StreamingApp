using System.Security.Claims;
using Infrastructure.SignalR.Hubs.Constants;
using Microsoft.AspNetCore.SignalR;
using SignalR.Hubs.Stream.Client.Abstractions.Services;

namespace SignalR.Hubs.Stream;

public sealed class StreamHub : Hub<IStreamHub>
{
    private readonly IStreamHubClientService _hubClientService;
    private readonly string _userId;

    public StreamHub(IStreamHubClientService hubClientService)
    {
        _hubClientService = hubClientService;
        _userId = Context?.User?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ??
                  StreamHubConstant.AnonymousUser;
    }

    public override async Task OnConnectedAsync()
    {
        await _hubClientService.OnConnectedAsync(_userId, Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _hubClientService.OnDisconnectedAsync(_userId, Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async ValueTask OnJoinedStreamAsync(string streamerName)
    {
        await _hubClientService.OnJoinedStreamAsync(streamerName, Context.ConnectionId);
    }

    public async ValueTask OnLeavedStreamAsync(string streamerName)
    {
        await _hubClientService.OnLeavedStreamAsync(streamerName, Context.ConnectionId);
    }

    // public async Task OnStreamStartedAsync(GetStreamDto streamDto)
    // {
    //     var streamerId = streamDto.User.Id.ToString();
    //
    //     var streamViewerConnectionIds = await _hubClientService.GetStreamViewerConnectionIds(streamerId);
    //
    //     await Clients.Clients(streamViewerConnectionIds).OnStreamStartedAsync(streamDto);
    // }
    //
    // public async ValueTask OnStreamEndAsync(GetStreamDto streamDto)
    // {
    //     var streamerId = streamDto.User.Id.ToString();
    //
    //     var streamViewerConnectionIds = await _hubClientService.GetStreamViewerConnectionIds(streamerId);
    //
    //     await Clients.Clients(streamViewerConnectionIds).OnStreamEndAsync(streamDto);
    // }
}