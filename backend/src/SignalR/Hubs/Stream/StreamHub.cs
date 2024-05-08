using Microsoft.AspNetCore.SignalR;
using SignalR.Contracts;
using SignalR.Hubs.Stream.Client.Abstractions.Services;
using SignalR.Hubs.Stream.Server.Abstractions;

namespace SignalR.Hubs.Stream;

public sealed class StreamHub : Hub<IStreamHub>
{
    private readonly IStreamHubClientService _hubClientService;
    private readonly IStreamHubServerService _hubServerService;

    public StreamHub(IStreamHubClientService hubClientService, IStreamHubServerService hubServerService)
    {
        _hubClientService = hubClientService;
        _hubServerService = hubServerService;
    }

    public override async Task OnConnectedAsync()
    {
        var tasks = new List<Task>()
        {
            base.OnConnectedAsync(),
            _hubClientService.OnConnectedToHubAsync(Context.ConnectionId).AsTask(),
        };
        await Task.WhenAll(tasks);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var tasks = new List<Task>
        {
            _hubClientService.OnDisconnectedFromHubAsync(Context.ConnectionId).AsTask(),
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

    public async ValueTask OnStreamChatMessageSendAsync(string streamerName, StreamChatMessageDto streamChatMessageDto)
    {
        await _hubServerService.OnStreamChatMessageSendAsync(streamerName, streamChatMessageDto);
    }
}