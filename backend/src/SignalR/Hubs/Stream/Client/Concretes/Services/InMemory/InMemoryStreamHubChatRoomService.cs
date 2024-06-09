using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using SignalR.Extensions;
using SignalR.Hubs.Stream.Client.Abstractions.Services;
using SignalR.Hubs.Stream.Shared;
using SignalR.Models;

namespace SignalR.Hubs.Stream.Client.Concretes.Services.InMemory;

public sealed class InMemoryStreamHubChatRoomService : IStreamHubChatRoomService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly bool _isAuthenticated;
    private readonly ConcurrentDictionary<string, HubConnectionInfo> _streamViewers;

    public InMemoryStreamHubChatRoomService(IStreamHubState hubState, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _isAuthenticated = httpContextAccessor.HttpContext?.IsAuthenticated() ?? false;
        _streamViewers = hubState.StreamViewers;
    }

    public ValueTask<HubConnectionInfo?> GetStreamViewers(string streamerName)
    {
        if (!_streamViewers.TryGetValue(streamerName, out var hubConnectionInfo))
        {
            return ValueTask.FromResult<HubConnectionInfo?>(null);
        }

        return ValueTask.FromResult(hubConnectionInfo)!;
    }

    public ValueTask<IEnumerable<HubConnectionId>> GetStreamViewerConnectionIds(string streamerName,
        IEnumerable<string> userIds)
    {
        if (!_streamViewers.TryGetValue(streamerName, out var hubConnectionInfo))
        {
            return ValueTask.FromResult<IEnumerable<string>>([]);
        }

        var connectionIds = hubConnectionInfo!
            .Users
            .Where(kvp => userIds.Any(id => id == kvp.Value.Id))
            .Select(kvp => kvp.Key);

        return ValueTask.FromResult(connectionIds);
    }

    public ValueTask<IEnumerable<string>> GetStreamViewerConnectionIds(string streamerName)
    {
        if (!_streamViewers.TryGetValue(streamerName, out var hubConnectionInfo))
        {
            ValueTask.FromResult<IEnumerable<string>>([]);
        }

        return ValueTask.FromResult(hubConnectionInfo!.GetAllConnectionIds());
    }

    public ValueTask<string?> GetStreamViewerConnectionId(string streamerName, Guid userId)
    {
        if (!_streamViewers.TryGetValue(streamerName, out var hubConnectionInfo))
        {
            ValueTask.FromResult<string>(null!);
        }

        var userConnectionId = hubConnectionInfo!
            .Users
            .Where(kvp => kvp.Value.Id == userId.ToString())
            .Select(kvp => kvp.Key)
            .SingleOrDefault();

        return ValueTask.FromResult(userConnectionId);
    }

    public ValueTask<IEnumerable<HubUserDto>> GetStreamViewersAsync(string streamerName)
    {
        if (!_streamViewers.TryGetValue(streamerName, out var hubConnectionInfo))
        {
            return ValueTask.FromResult<IEnumerable<HubUserDto>>([]);
        }

        return ValueTask.FromResult(hubConnectionInfo.Users.Select(kvp => kvp.Value).AsEnumerable());
    }

    public ValueTask<int> GetStreamViewersCountAsync(string streamerName)
    {
        if (!_streamViewers.TryGetValue(streamerName, out var hubConnectionInfo))
        {
            return ValueTask.FromResult(0);
        }

        return ValueTask.FromResult(
            hubConnectionInfo.Users.Count + hubConnectionInfo.AnonymousUserConnectionIds.Count);
    }

    public ValueTask OnJoinedStreamAsync(string streamerName, string connectionId)
    {
        var hubConnectionInfo = _streamViewers.GetOrAdd(streamerName, HubConnectionInfo.Create());

        return _isAuthenticated switch
        {
            true => this.OnUserJoinedStreamAsync(hubConnectionInfo, connectionId),
            false => this.OnAnonymousUserJoinedStreamAsync(hubConnectionInfo, connectionId)
        };
    }

    private ValueTask OnUserJoinedStreamAsync(HubConnectionInfo hubConnectionInfo, string connectionId)
    {
        _ = hubConnectionInfo.Users.GetOrAdd(connectionId, HubUserDto.Create(_httpContextAccessor.HttpContext!.User));

        return ValueTask.CompletedTask;
    }

    private ValueTask OnAnonymousUserJoinedStreamAsync(HubConnectionInfo hubConnectionInfo, string connectionId)
    {
        hubConnectionInfo.AnonymousUserConnectionIds.Add(connectionId);

        return ValueTask.CompletedTask;
    }


    public ValueTask OnLeavedStreamAsync(string streamerName, string connectionId)
    {
        var exists = _streamViewers.Keys.Any(key => key == streamerName);

        if (!exists)
        {
            return ValueTask.CompletedTask;
        }

        var hubConnectionInfo = _streamViewers[streamerName];


        return _isAuthenticated switch
        {
            true => this.OnUserLeavedStreamAsync(streamerName, hubConnectionInfo, connectionId),
            false => this.OnAnonymousUserLeavedStreamAsync(streamerName, hubConnectionInfo, connectionId)
        };
    }

    private ValueTask OnUserLeavedStreamAsync(string streamerName, HubConnectionInfo hubConnectionInfo,
        string connectionId)
    {
        hubConnectionInfo.Users.TryRemove(connectionId, out _);

        if (hubConnectionInfo.Users.Count is 0)
        {
            _streamViewers.Remove(streamerName, out _);
        }

        return ValueTask.CompletedTask;
    }

    private ValueTask OnAnonymousUserLeavedStreamAsync(string streamerName, HubConnectionInfo hubConnectionInfo,
        string connectionId)
    {
        hubConnectionInfo.AnonymousUserConnectionIds.Remove(connectionId);

        if (hubConnectionInfo.AnonymousUserConnectionIds.Count is 0)
        {
            _streamViewers.Remove(streamerName, out _);
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> OnDisconnectedFromChatRoomsAsync(string connectionId)
    {
        return _isAuthenticated switch
        {
            true => this.OnUserDisconnectedFromChatRoomsAsync(connectionId),
            false => this.OnAnonymousUserDisconnectedFromChatRoomsAsync(connectionId)
        };
    }

    private ValueTask<bool> OnUserDisconnectedFromChatRoomsAsync(string connectionId)
    {
        var keys = _streamViewers.Where(kvp =>
                kvp.Value.Users.TryGetValue(connectionId, out _))
            .Select(kvp => kvp.Key);

        foreach (string key in keys)
        {
            this.OnLeavedStreamAsync(key, connectionId);
        }

        return ValueTask.FromResult(true);
    }

    private ValueTask<bool> OnAnonymousUserDisconnectedFromChatRoomsAsync(string connectionId)
    {
        var keys = _streamViewers.Where(kvp => kvp.Value.AnonymousUserConnectionIds.Any(id => id == connectionId))
            .Select(kvp => kvp.Key);

        foreach (string key in keys)
        {
            this.OnLeavedStreamAsync(key, connectionId);
        }

        return ValueTask.FromResult(true);
    }
}