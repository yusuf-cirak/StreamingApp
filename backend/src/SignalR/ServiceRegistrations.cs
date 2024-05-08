using Microsoft.Extensions.DependencyInjection;
using SignalR.Hubs.Stream.Client.Abstractions.Services;
using SignalR.Hubs.Stream.Client.Concretes.Services.InMemory;
using SignalR.Hubs.Stream.Server.Abstractions;
using SignalR.Hubs.Stream.Server.Concretes;
using SignalR.Hubs.Stream.Shared;
using SignalR.Hubs.Stream.Shared.InMemory;

namespace SignalR;

public static class ServiceRegistrations
{
    public static void AddSignalrServices(this IServiceCollection services)
    {
        services.AddSingleton<IStreamHubState, InMemoryStreamHubState>();

        services.AddScoped<IStreamHubUserService, InMemoryStreamHubUserService>();
        services.AddScoped<IStreamHubChatRoomService, InMemoryStreamHubChatRoomService>();

        services.AddScoped<IStreamHubClientService, InMemoryStreamHubClientService>();
        services.AddScoped<IStreamHubServerService, InMemoryStreamHubServerService>();
        
    }
}