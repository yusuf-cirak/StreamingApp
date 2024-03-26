namespace Application.Abstractions.Hubs;

public interface IStreamHubViewerService
{
    ValueTask OnJoinedStreamAsync(string streamerId, string userId);
    ValueTask OnLeavedStreamAsync(string streamerId, string userId);
}