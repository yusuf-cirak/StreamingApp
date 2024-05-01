using Application.Contracts.StreamOptions;
using Application.Contracts.Streams;
using SignalR.Contracts;

namespace SignalR.Hubs.Stream.Server.Abstractions;

public interface IStreamHubServerService
{
    Task OnStreamStartedAsync(GetStreamDto streamDto);
    Task OnStreamEndAsync(string streamerName);
    Task OnStreamChatOptionsChangedAsync(GetStreamChatSettingsDto streamChatSettingsDto, string streamerName);
    Task OnStreamChatMessageSendAsync(string streamerName, StreamChatMessageDto streamChatMessageDto);
}