﻿using SharedKernel;
using SignalR.Hubs.Stream.Client.Abstractions.Services;
using SignalR.Models;

namespace SignalR.Hubs.Stream.Client.Concretes.Services.InMemory;

public sealed class InMemoryStreamHubClientService : IStreamHubClientService
{
    private readonly IStreamHubUserService _streamHubUserService;
    private readonly IStreamHubChatRoomService _streamHubChatRoomService;

    public InMemoryStreamHubClientService(IStreamHubUserService streamHubUserService,
        IStreamHubChatRoomService streamHubChatRoomService)
    {
        _streamHubUserService = streamHubUserService;
        _streamHubChatRoomService = streamHubChatRoomService;
    }

    public ValueTask<bool> OnConnectedToHubAsync(string connectionId)
    {
        return _streamHubUserService.OnConnectedToHubAsync(connectionId);
    }

    public ValueTask<bool> OnDisconnectedFromHubAsync(string connectionId)
    {
        return _streamHubUserService.OnDisconnectedFromHubAsync(connectionId);
    }

    public ValueTask<IEnumerable<string>> GetStreamViewerConnectionIds(string streamerName)
    {
        return _streamHubChatRoomService.GetStreamViewerConnectionIds(streamerName);
    }

    public ValueTask<IEnumerable<HubUserDto>> GetStreamViewersAsync(string streamerName)
    {
        return _streamHubChatRoomService.GetStreamViewersAsync(streamerName);
    }

    public ValueTask OnJoinedStreamAsync(string streamerName, string connectionId)
    {
        return _streamHubChatRoomService.OnJoinedStreamAsync(streamerName, connectionId);
    }

    public ValueTask OnLeavedStreamAsync(string streamerName, string connectionId)
    {
        return _streamHubChatRoomService.OnLeavedStreamAsync(streamerName, connectionId);
    }

    public ValueTask<bool> OnDisconnectedFromChatRoomsAsync(string connectionId)
    {
        return _streamHubChatRoomService.OnDisconnectedFromChatRoomsAsync(connectionId);
    }
}