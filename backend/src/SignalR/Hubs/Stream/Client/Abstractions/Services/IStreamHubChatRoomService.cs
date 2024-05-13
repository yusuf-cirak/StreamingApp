using SignalR.Models;

namespace SignalR.Hubs.Stream.Client.Abstractions.Services;

public interface IStreamHubChatRoomService
{
    ValueTask<IEnumerable<string>> GetStreamViewerConnectionIds(string streamerName);
    ValueTask<IEnumerable<HubUserDto>> GetStreamViewersAsync(string streamerName);
    ValueTask<int> GetStreamViewersCountAsync(string streamerName);
    ValueTask OnJoinedStreamAsync(string streamerName, string connectionId);
    ValueTask OnLeavedStreamAsync(string streamerName, string connectionId);
    ValueTask<bool> OnDisconnectedFromChatRoomsAsync(string connectionId);
}