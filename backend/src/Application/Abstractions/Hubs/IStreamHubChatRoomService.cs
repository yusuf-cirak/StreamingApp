namespace Application.Abstractions.Hubs;

public interface IStreamHubChatRoomService
{
    ValueTask<HashSet<string>> GetStreamViewerConnectionIds(string streamerId);
    ValueTask OnJoinedStreamAsync(string streamerName, string connectionId);
    ValueTask OnLeavedStreamAsync(string streamerName, string connectionId);
}