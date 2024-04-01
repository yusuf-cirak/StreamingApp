using System.Collections.Concurrent;
using Application.Abstractions.Hubs;
using Infrastructure.SignalR.Hubs.Constants;
using SharedKernel;

namespace Infrastructure.SignalR.Hubs.Services.InMemory;

public sealed class InMemoryStreamHubUserService : IStreamHubUserService
{
    private readonly ConcurrentDictionary<string, HashSet<string>> _onlineUsers = new()
    {
        [StreamHubConstant.AnonymousUser] = new HashSet<string>()
    };

    public ValueTask<bool> OnConnectedAsync(string userId, string connectionId)
    {
        var userConnectionIds = this.GetUserConnectionIds(userId);

        userConnectionIds.Add(connectionId);

        return ValueTask.FromResult(true);
    }

    public ValueTask<bool> OnDisconnectedAsync(string userId, string connectionId)
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