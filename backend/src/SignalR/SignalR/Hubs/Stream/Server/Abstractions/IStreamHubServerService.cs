using Application.Contracts.StreamOptions;
using Application.Contracts.Streams;

namespace SignalR.Hubs.Stream.Server.Abstractions;

public interface IStreamHubServerService
{
    Task OnStreamStartedAsync(GetStreamDto streamDto);
    Task OnStreamEndAsync(string streamerName);
    Task OnStreamChatOptionsChangedAsync(GetStreamChatSettingsDto streamChatSettingsDto, string streamerName);
}