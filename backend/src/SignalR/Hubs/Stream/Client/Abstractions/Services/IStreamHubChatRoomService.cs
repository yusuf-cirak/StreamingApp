namespace SignalR.Hubs.Stream.Client.Abstractions.Services;

public interface IStreamHubChatRoomService
{
    ValueTask<HashSet<string>> GetStreamViewerConnectionIds(string streamerName);
    ValueTask OnJoinedStreamAsync(string streamerName, string connectionId);
    ValueTask OnLeavedStreamAsync(string streamerName, string connectionId);
    ValueTask<bool> OnDisconnectedFromChatRoomsAsync(string connectionId);
}