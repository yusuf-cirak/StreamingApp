namespace SignalR.Hubs.Stream;

public interface IStreamHub
{
    ValueTask OnJoinedStreamAsync(string streamerId);
    ValueTask OnLeavedStreamAsync(string streamerId);
}