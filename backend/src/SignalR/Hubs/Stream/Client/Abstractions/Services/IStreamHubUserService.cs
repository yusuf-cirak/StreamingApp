namespace SignalR.Hubs.Stream.Client.Abstractions.Services;

public interface IStreamHubUserService
{
    ValueTask<bool> OnConnectedToHubAsync(string connectionId);
    ValueTask<bool> OnDisconnectedFromHubAsync(string connectionId);
}