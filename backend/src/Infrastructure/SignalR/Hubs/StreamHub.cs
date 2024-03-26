using Application.Abstractions.Hubs;
using Application.Common.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.SignalR.Hubs;

public sealed class StreamHub : Hub<IStreamHub>
{
    private readonly IStreamHubService _hubService;

    public StreamHub(IStreamHubService hubService)
    {
        _hubService = hubService;
    }

    public override async Task OnConnectedAsync()
    {
        await _hubService.OnConnectedAsync(Context.User.GetUserId(), Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await _hubService.OnDisconnectedAsync(Context.User.GetUserId());
        await base.OnDisconnectedAsync(exception);
    }

    public async ValueTask OnJoinedStreamAsync(string streamerId, string userId)
    {
        await _hubService.OnJoinedStreamAsync(streamerId, userId);
    }
    
    public async ValueTask OnLeavedStreamAsync(string streamerId, string userId)
    {
        await _hubService.OnLeavedStreamAsync(streamerId, userId);
    }
}