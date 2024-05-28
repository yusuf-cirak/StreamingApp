using Application.Contracts.StreamOptions;
using Application.Contracts.Streams;
using Application.Contracts.Users;
using Microsoft.AspNetCore.SignalR;
using SignalR.Constants;
using SignalR.Contracts;
using SignalR.Hubs.Stream.Client.Abstractions.Services;
using SignalR.Hubs.Stream.Server.Abstractions;

namespace SignalR.Hubs.Stream.Server.Concretes;

public sealed class InMemoryStreamHubServerService : IStreamHubServerService
{
    private readonly IHubContext<StreamHub> _hubContext;
    private readonly IStreamHubChatRoomService _hubChatRoomService;

    public InMemoryStreamHubServerService(IHubContext<StreamHub> hubContext,
        IStreamHubChatRoomService hubChatRoomService)
    {
        _hubContext = hubContext;
        _hubChatRoomService = hubChatRoomService;
    }

    public async Task OnStreamStartedAsync(GetStreamDto streamDto)
    {
        var streamerName = streamDto.User.Username;

        var streamViewerConnectionIds = await _hubChatRoomService.GetStreamViewerConnectionIds(streamerName);

        await Task.Delay(TimeSpan.FromSeconds(10));

        await _hubContext.Clients.Clients(streamViewerConnectionIds)
            .SendAsync(StreamHubConstant.Method.OnStreamStartedAsync, streamDto, default);
    }

    public async Task OnStreamEndAsync(string streamerName)
    {
        var streamViewerConnectionIds = await _hubChatRoomService.GetStreamViewerConnectionIds(streamerName);

        await _hubContext.Clients.Clients(streamViewerConnectionIds)
            .SendAsync(StreamHubConstant.Method.OnStreamEndAsync, streamerName, default);
    }

    public async Task OnStreamChatOptionsChangedAsync(GetStreamChatSettingsDto streamChatSettingsDto,
        string streamerName)
    {
        var streamViewerConnectionIds =
            await _hubChatRoomService.GetStreamViewerConnectionIds(streamerName);

        await _hubContext.Clients.Clients(streamViewerConnectionIds)
            .SendAsync(StreamHubConstant.Method.OnStreamChatOptionsChangedAsync, streamChatSettingsDto, default);
    }

    public async Task OnStreamChatMessageSendAsync(string streamerName, StreamChatMessageDto streamChatMessageDto)
    {
        var streamViewerConnectionIds =
            await _hubChatRoomService.GetStreamViewerConnectionIds(streamerName);

        await _hubContext.Clients.Clients(streamViewerConnectionIds)
            .SendAsync(StreamHubConstant.Method.OnStreamChatMessageSendAsync, streamChatMessageDto);
    }

    public async Task OnBlockFromStreamAsync(GetUserDto streamer, List<Guid> blockUserIds, bool isBlocked)
    {
        var ids = blockUserIds.Select(id => id.ToString());
        var streamViewerConnectionIds = (await _hubChatRoomService
            .GetStreamViewerConnectionIds(streamer.Username, ids));


        var viewerConnectionIds = streamViewerConnectionIds as HubConnectionId[] ?? streamViewerConnectionIds.ToArray();
        
        if (!viewerConnectionIds.Any())
        {
            return;
        }
        
        await _hubContext.Clients.Clients(viewerConnectionIds).SendAsync(StreamHubConstant.Method.OnBlockFromStreamAsync, new StreamBlockUserDto(streamer.Id, isBlocked));
    }
}