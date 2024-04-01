using Application.Abstractions.Hubs;
using Application.Common.Extensions;
using Application.Features.Streams.Dtos;
using Infrastructure.SignalR.Hubs.Constants;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.SignalR.Hubs;

public sealed class StreamHub : Hub<IStreamHub>
{
    private readonly IStreamHubService _hubService;
    private readonly string _userId;

    public StreamHub(IStreamHubService hubService)
    {
        _hubService = hubService;
        _userId = Context?.User.GetUserId() ?? StreamHubConstant.AnonymousUser;
    }

    public override async Task OnConnectedAsync()
    {
        await _hubService.OnConnectedAsync(_userId, Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await _hubService.OnDisconnectedAsync(_userId, Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async ValueTask OnJoinedStreamAsync(string streamerName)
    {
        await _hubService.OnJoinedStreamAsync(streamerName, Context.ConnectionId);
    }

    public async ValueTask OnLeavedStreamAsync(string streamerName)
    {
        await _hubService.OnLeavedStreamAsync(streamerName, Context.ConnectionId);
    }

    public async ValueTask OnStreamStartedAsync(GetStreamDto streamDto)
    {
        var streamerId = streamDto.User.Id.ToString();

        var streamViewerConnectionIds = await _hubService.GetStreamViewerConnectionIds(streamerId);

        await Clients.Clients(streamViewerConnectionIds).OnStreamStartedAsync(streamDto);
    }

    public async ValueTask OnStreamEndAsync(GetStreamDto streamDto)
    {
        var streamerId = streamDto.User.Id.ToString();

        var streamViewerConnectionIds = await _hubService.GetStreamViewerConnectionIds(streamerId);

        await Clients.Clients(streamViewerConnectionIds).OnStreamEndAsync(streamDto);
    }
}