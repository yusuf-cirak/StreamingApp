using Application.Contracts.Streams;

namespace SignalR.Abstractions;

public interface IStreamHub
{
    Task OnStreamStartedAsync(GetStreamDto streamDto);
    Task OnStreamEndAsync(GetStreamDto streamDto);
    ValueTask OnJoinedStreamAsync(string streamerId);
    ValueTask OnLeavedStreamAsync(string streamerId);
}