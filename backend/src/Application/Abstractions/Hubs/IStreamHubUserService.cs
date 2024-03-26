namespace Application.Abstractions.Hubs;

public interface IStreamHubUserService
{
    ValueTask<bool> OnConnectedAsync(string userId, string connectionId);
    ValueTask<bool> OnDisconnectedAsync(string userId);
    ValueTask<Result<string,Error>> GetUserIdByConnectionIdAsync(string connectionId);
}