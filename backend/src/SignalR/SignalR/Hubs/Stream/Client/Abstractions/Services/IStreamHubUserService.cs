﻿using SharedKernel;

namespace SignalR.Hubs.Stream.Client.Abstractions.Services;

public interface IStreamHubUserService
{
    ValueTask<bool> OnConnectedAsync(string userId, string connectionId);
    ValueTask<bool> OnDisconnectedAsync(string userId,string connectionId);
    ValueTask<Result<string,Error>> GetUserIdByConnectionIdAsync(string connectionId);
}