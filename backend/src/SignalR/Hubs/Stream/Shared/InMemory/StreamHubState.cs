using System.Collections.Concurrent;
using SignalR.Constants;
using SignalR.Models;

namespace SignalR.Hubs.Stream.Shared.InMemory;

public sealed class InMemoryStreamHubState : IStreamHubState
{
    // public ConcurrentDictionary<string, HashSet<string>> StreamViewers { get; } =
    //     new();
    //
    // public ConcurrentDictionary<string, HashSet<string>> OnlineUsers { get; } =
    //     new()
    //     {
    //         [StreamHubConstant.AnonymousUser] = new()
    //     };

    public ConcurrentDictionary<StreamerName, HubConnectionInfo> StreamViewers { get; } = new();
    public HubConnectionInfo OnlineUsers { get; } = HubConnectionInfo.Create();
}