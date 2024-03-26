namespace Infrastructure.SignalR.Hubs;

public interface IStreamHub
{
    ValueTask OnJoinedStreamAsync(string streamerId, string userId);
    ValueTask OnLeavedStreamAsync(string streamerId, string userId);
}