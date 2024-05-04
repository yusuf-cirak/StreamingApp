using SharedKernel;

namespace SignalR.Hubs.Stream.Client.Abstractions.Services;

public interface IStreamHubUserService
{
    ValueTask<bool> OnConnectedToHubAsync(string userId, string connectionId);
    ValueTask<bool> OnDisconnectedFromHubAsync(string userId,string connectionId);
    ValueTask<Result<string,Error>> GetUserIdByConnectionIdAsync(string connectionId);
}