using System.Collections.Concurrent;
using SharedKernel;

namespace SignalR.Models;

public sealed class HubConnectionInfo
{
    public ConcurrentDictionary<HubConnectionId, HubUserDto> Users { get; } = new();
    public ConcurrentList<HubConnectionId> AnonymousUserConnectionIds { get; } = new();


    private HubConnectionInfo()
    {
    }

    public static HubConnectionInfo Create() => new();


    public IEnumerable<string> GetAllConnectionIds() =>
        Users.Keys.Concat(AnonymousUserConnectionIds);
}