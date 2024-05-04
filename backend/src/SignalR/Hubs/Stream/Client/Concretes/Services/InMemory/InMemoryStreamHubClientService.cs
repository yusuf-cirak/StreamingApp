using SharedKernel;
using SignalR.Hubs.Stream.Client.Abstractions.Services;

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

    public ValueTask<bool> OnConnectedToHubAsync(string userId, string connectionId)
    {
        return _streamHubUserService.OnConnectedToHubAsync(userId, connectionId);
    }

    public ValueTask<bool> OnDisconnectedFromHubAsync(string userId, string connectionId)
    {
        return _streamHubUserService.OnDisconnectedFromHubAsync(userId, connectionId);
    }

    public ValueTask<Result<string, Error>> GetUserIdByConnectionIdAsync(string connectionId)
    {
        return _streamHubUserService.GetUserIdByConnectionIdAsync(connectionId);
    }

    public ValueTask<HashSet<string>> GetStreamViewerConnectionIds(string streamerName)
    {
        return _streamHubChatRoomService.GetStreamViewerConnectionIds(streamerName);
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