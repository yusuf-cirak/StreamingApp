using Application.Features.Streams.Dtos;

namespace Infrastructure.SignalR.Hubs;

public interface IStreamHub
{
    Task OnStreamStartedAsync(GetStreamDto streamDto);
    Task OnStreamEndAsync(GetStreamDto streamDto);
    ValueTask OnJoinedStreamAsync(string streamerId);
    ValueTask OnLeavedStreamAsync(string streamerId);
}