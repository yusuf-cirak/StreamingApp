using System.Collections.Concurrent;
using SignalR.Models;

namespace SignalR.Hubs.Stream.Shared;

public interface IStreamHubState
{
    ConcurrentDictionary<StreamerName, HubConnectionInfo> StreamViewers { get; }
    HubConnectionInfo OnlineUsers { get; }
}