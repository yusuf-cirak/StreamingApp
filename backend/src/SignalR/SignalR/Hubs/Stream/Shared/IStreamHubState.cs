using System.Collections.Concurrent;

namespace SignalR.Hubs.Stream.Shared;

public interface IStreamHubState
{
    ConcurrentDictionary<string, HashSet<string>> StreamViewers { get; }
    ConcurrentDictionary<string, HashSet<string>> OnlineUsers { get; }
}