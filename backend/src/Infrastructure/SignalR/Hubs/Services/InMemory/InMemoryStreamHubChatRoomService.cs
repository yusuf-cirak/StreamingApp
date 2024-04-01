using System.Collections.Concurrent;
using Application.Abstractions.Hubs;

namespace Infrastructure.SignalR.Hubs.Services.InMemory;

public sealed class InMemoryStreamHubChatRoomService : IStreamHubChatRoomService
{
    private readonly ConcurrentDictionary<string, HashSet<string>> _streamViewers = new();


    public ValueTask<HashSet<string>> GetStreamViewerConnectionIds(string streamerId)
    {
        return ValueTask.FromResult(_streamViewers.GetOrAdd(streamerId, new HashSet<string>()));
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
}