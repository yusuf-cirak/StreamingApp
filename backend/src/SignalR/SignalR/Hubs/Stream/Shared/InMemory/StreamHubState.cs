using System.Collections.Concurrent;
using Infrastructure.SignalR.Hubs.Constants;

namespace SignalR.Hubs.Stream.Shared.InMemory;

public sealed class InMemoryStreamHubState : IStreamHubState
{
    public ConcurrentDictionary<string, HashSet<string>> StreamViewers { get; } =
        new();

    public ConcurrentDictionary<string, HashSet<string>> OnlineUsers { get; } =
        new()
        {
            [StreamHubConstant.AnonymousUser] = new()
        };
}