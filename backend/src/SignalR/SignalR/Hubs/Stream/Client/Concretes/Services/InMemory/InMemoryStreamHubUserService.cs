using System.Collections.Concurrent;
using SharedKernel;
using SignalR.Hubs.Stream.Client.Abstractions.Services;
using SignalR.Hubs.Stream.Shared;

namespace SignalR.Hubs.Stream.Client.Concretes.Services.InMemory;

public sealed class InMemoryStreamHubUserService : IStreamHubUserService
{
    private readonly ConcurrentDictionary<string, HashSet<string>> _onlineUsers;

    public InMemoryStreamHubUserService(IStreamHubState hubState)
    {
        _onlineUsers = hubState.OnlineUsers;
    }

    public ValueTask<bool> OnConnectedToHubAsync(string userId, string connectionId)
    {
        var userConnectionIds = this.GetUserConnectionIds(userId);

        userConnectionIds.Add(connectionId);

        return ValueTask.FromResult(true);
    }

    public ValueTask<bool> OnDisconnectedFromHubAsync(string userId, string connectionId)
    {
        var userConnectionIds = this.GetUserConnectionIds(userId);
        if (userConnectionIds.Count == 1)
        {
            return ValueTask.FromResult(_onlineUsers.TryRemove(userId, out _));
        }

        return ValueTask.FromResult(userConnectionIds.Remove(connectionId));
    }

    public ValueTask<Result<string, Error>> GetUserIdByConnectionIdAsync(string connectionId)
    {
        var onlineUser = _onlineUsers.SingleOrDefault(onlineUser => onlineUser.Value.Any(id => id == connectionId));

        if (onlineUser.Value != default)
        {
            return ValueTask.FromResult<Result<string, Error>>(onlineUser.Key);
        }

        return ValueTask.FromResult<Result<string, Error>>(Error.Create("User.NotFound", "User is not found"));
    }

    private HashSet<string> GetUserConnectionIds(string userId) =>
        _onlineUsers.GetOrAdd(userId, new HashSet<string>());
}