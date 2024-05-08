using Microsoft.AspNetCore.Http;
using SignalR.Extensions;
using SignalR.Hubs.Stream.Client.Abstractions.Services;
using SignalR.Hubs.Stream.Shared;
using SignalR.Models;

namespace SignalR.Hubs.Stream.Client.Concretes.Services.InMemory;

public sealed class InMemoryStreamHubUserService : IStreamHubUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly bool _isAuthenticated;
    private readonly HubConnectionInfo _onlineUsers;

    public InMemoryStreamHubUserService(IStreamHubState hubState, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _isAuthenticated = httpContextAccessor.HttpContext?.IsAuthenticated() ?? false;
        _onlineUsers = hubState.OnlineUsers;
    }

    public ValueTask<bool> OnConnectedToHubAsync(string connectionId)
    {
        return _isAuthenticated switch
        {
            true => this.OnUserConnectedToHubAsync(connectionId),
            false => this.OnAnonymousUserConnectedToHubAsync(connectionId)
        };
    }

    private ValueTask<bool> OnUserConnectedToHubAsync(string connectionId)
    {
        var user = HubUserDto.Create(_httpContextAccessor.HttpContext!.User);
        _ = _onlineUsers.Users.GetOrAdd(connectionId, user);

        return ValueTask.FromResult(true);
    }

    private ValueTask<bool> OnAnonymousUserConnectedToHubAsync(string connectionId)
    {
        _onlineUsers.AnonymousUserConnectionIds.Add(connectionId);

        return ValueTask.FromResult(true);
    }

    public ValueTask<bool> OnDisconnectedFromHubAsync(string connectionId)
    {
        return _isAuthenticated switch
        {
            true => this.OnUserDisconnectedFromHubAsync(connectionId),
            false => this.OnAnonymousUserDisconnectedFromHubAsync(connectionId)
        };
    }

    private ValueTask<bool> OnUserDisconnectedFromHubAsync(string connectionId)
    {
        var result = _onlineUsers.Users.TryRemove(connectionId, out _);

        return ValueTask.FromResult(result);
    }

    private ValueTask<bool> OnAnonymousUserDisconnectedFromHubAsync(string connectionId)
    {
        var result = _onlineUsers.AnonymousUserConnectionIds.Remove(connectionId);

        return ValueTask.FromResult(result);
    }
}