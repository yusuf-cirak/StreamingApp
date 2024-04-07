using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using SignalR.Constants;
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
        var tasks = new List<Task>()
        {
            _hubClientService.OnConnectedToHubAsync(_userId, Context.ConnectionId).AsTask(),
            base.OnConnectedAsync()
        };
        await Task.WhenAll(tasks);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var tasks = new List<Task>
        {
            _hubClientService.OnDisconnectedFromHubAsync(_userId, Context.ConnectionId).AsTask(),
            _hubClientService.OnDisconnectedFromChatRoomsAsync(Context.ConnectionId).AsTask(),
            base.OnDisconnectedAsync(exception)
        };
        await Task.WhenAll(tasks);
    }

    public async ValueTask OnJoinedStreamAsync(string streamerName)
    {
        await _hubClientService.OnJoinedStreamAsync(streamerName, Context.ConnectionId);
    }

    public async ValueTask OnLeavedStreamAsync(string streamerName)
    {
        await _hubClientService.OnLeavedStreamAsync(streamerName, Context.ConnectionId);
    }
}