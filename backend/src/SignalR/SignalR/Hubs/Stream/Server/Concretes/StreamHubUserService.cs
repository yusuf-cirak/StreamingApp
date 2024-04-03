using Microsoft.AspNetCore.SignalR;
using SignalR.Hubs.Stream.Server.Abstractions;

namespace SignalR.Hubs.Stream.Server.Concretes;

public sealed class StreamHubUserService : IStreamHubServerService
{
    private readonly IHubContext<StreamHub> _hubContext;

    public StreamHubUserService(IHubContext<StreamHub> hubContext)
    {
        _hubContext = hubContext;
    }
}