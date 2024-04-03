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

    public ValueTask<bool> OnConnectedAsync(string userId, string connectionId)
    {
        return _streamHubUserService.OnConnectedAsync(userId, connectionId);
    }

    public ValueTask<bool> OnDisconnectedAsync(string userId, string connectionId)
    {
        return _streamHubUserService.OnDisconnectedAsync(userId, connectionId);
    }

    public ValueTask<Result<string, Error>> GetUserIdByConnectionIdAsync(string connectionId)
    {
        return _streamHubUserService.GetUserIdByConnectionIdAsync(connectionId);
    }

    public ValueTask<HashSet<string>> GetStreamViewerConnectionIds(string streamerId)
    {
        return _streamHubChatRoomService.GetStreamViewerConnectionIds(streamerId);
    }

    public ValueTask OnJoinedStreamAsync(string streamerName, string connectionId)
    {
        return _streamHubChatRoomService.OnJoinedStreamAsync(streamerName, connectionId);
    }

    public ValueTask OnLeavedStreamAsync(string streamerName, string connectionId)
    {
        return _streamHubChatRoomService.OnLeavedStreamAsync(streamerName, connectionId);
    }
}