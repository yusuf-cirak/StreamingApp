using System.Collections.Concurrent;
using Application.Abstractions.Hubs;

namespace Infrastructure.Services.Hub.Streams.InMemory;

public sealed class InMemoryStreamHubViewerService : IStreamHubViewerService
{
    private readonly ConcurrentDictionary<string, HashSet<string>> _streamViewers = new();

    public ValueTask OnJoinedStreamAsync(string streamerId, string userId)
    {
        var exists = _streamViewers.Keys.Any(key => key == streamerId);

        if (!exists)
        {
            _streamViewers[streamerId] = new();
        }

        _streamViewers[streamerId].Add(userId);

        return ValueTask.CompletedTask;
    }

    public ValueTask OnLeavedStreamAsync(string streamerId, string userId)
    {
        var exists = _streamViewers.Keys.Any(key => key == streamerId);

        if (!exists)
        {
            return ValueTask.CompletedTask;
        }

        var viewers = _streamViewers[streamerId];

        viewers.Remove(userId);

        if (viewers.Count == 0)
        {
            _streamViewers.Remove(streamerId, out _);
        }

        return ValueTask.CompletedTask;
    }
}