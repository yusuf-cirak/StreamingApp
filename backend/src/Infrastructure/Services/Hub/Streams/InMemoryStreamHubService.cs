using Application.Abstractions.Hubs;
using SharedKernel;

namespace Infrastructure.Services.Hub.Streams;

public sealed class InMemoryStreamHubService : IStreamHubService
{
    private readonly IStreamHubUserService _streamHubUserService;
    private readonly IStreamHubViewerService _streamHubViewerService;

    public InMemoryStreamHubService(IStreamHubUserService streamHubUserService,
        IStreamHubViewerService streamHubViewerService)
    {
        _streamHubUserService = streamHubUserService;
        _streamHubViewerService = streamHubViewerService;
    }

    public ValueTask<bool> OnConnectedAsync(string userId, string connectionId)
    {
        return _streamHubUserService.OnConnectedAsync(userId, connectionId);
    }

    public ValueTask<bool> OnDisconnectedAsync(string userId)
    {
        return _streamHubUserService.OnDisconnectedAsync(userId);
    }

    public ValueTask<Result<string, Error>> GetUserIdByConnectionIdAsync(string connectionId)
    {
        return _streamHubUserService.GetUserIdByConnectionIdAsync(connectionId);
    }

    public ValueTask OnJoinedStreamAsync(string streamerId, string userId)
    {
        return _streamHubViewerService.OnJoinedStreamAsync(streamerId, userId);
    }

    public ValueTask OnLeavedStreamAsync(string streamerId, string userId)
    {
        return _streamHubViewerService.OnLeavedStreamAsync(streamerId, userId);
    }
}