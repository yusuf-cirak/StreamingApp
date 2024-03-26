using System.Collections.Concurrent;
using Application.Abstractions.Hubs;
using SharedKernel;

namespace Infrastructure.Services.Hub.Streams.InMemory;

public sealed class InMemoryStreamHubUserService : IStreamHubUserService
{
    private readonly ConcurrentDictionary<string, string> _onlineUsers = new();

    public ValueTask<bool> OnConnectedAsync(string userId, string connectionId)
    {
        return ValueTask.FromResult(_onlineUsers.TryAdd(userId, connectionId));
    }

    public ValueTask<bool> OnDisconnectedAsync(string userId)
    {
        return ValueTask.FromResult(_onlineUsers.TryRemove(userId, out _));
    }

    public ValueTask<Result<string, Error>> GetUserIdByConnectionIdAsync(string connectionId)
    {
        var onlineUser = _onlineUsers.SingleOrDefault(onlineUser => onlineUser.Value == connectionId);

        if (onlineUser.Value == connectionId)
        {
            return ValueTask.FromResult<Result<string, Error>>(onlineUser.Key);
        }

        return ValueTask.FromResult<Result<string, Error>>(Error.Create("User.NotFound", "User is not found"));
    }
}