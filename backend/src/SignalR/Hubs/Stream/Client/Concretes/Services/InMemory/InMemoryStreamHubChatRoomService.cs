using System.Collections.Concurrent;
using SignalR.Hubs.Stream.Client.Abstractions.Services;
using SignalR.Hubs.Stream.Shared;

namespace SignalR.Hubs.Stream.Client.Concretes.Services.InMemory;

public sealed class InMemoryStreamHubChatRoomService : IStreamHubChatRoomService
{
    private readonly ConcurrentDictionary<string, HashSet<string>> _streamViewers;

    public InMemoryStreamHubChatRoomService(IStreamHubState hubState)
    {
        _streamViewers = hubState.StreamViewers;
    }

    public ValueTask<HashSet<string>> GetStreamViewerConnectionIds(string streamerName)
    {
        return ValueTask.FromResult(_streamViewers.GetOrAdd(streamerName, new HashSet<string>()));
    }

    public ValueTask OnJoinedStreamAsync(string streamerName, string connectionId)
    {
        var streamViewers = _streamViewers.GetOrAdd(streamerName, new HashSet<string>());

        streamViewers.Add(connectionId);

        return ValueTask.CompletedTask;
    }

    public ValueTask OnLeavedStreamAsync(string streamerName, string connectionId)
    {
        var exists = _streamViewers.Keys.Any(key => key == streamerName);

        if (!exists)
        {
            return ValueTask.CompletedTask;
        }

        var viewers = _streamViewers[streamerName];

        viewers.Remove(connectionId);

        if (viewers.Count == 0)
        {
            _streamViewers.Remove(streamerName, out _);
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> OnDisconnectedFromChatRoomsAsync(string connectionId)
    {
        var keys = _streamViewers.Where(kvp => kvp.Value.Any(id => id == connectionId)).Select(kvp => kvp.Key).ToList();

        keys.ForEach(key => this.OnLeavedStreamAsync(key, connectionId));
        return ValueTask.FromResult(true);
    }
}