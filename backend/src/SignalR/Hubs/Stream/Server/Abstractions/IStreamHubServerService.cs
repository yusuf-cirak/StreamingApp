using Application.Contracts.StreamOptions;
using Application.Contracts.Streams;
using Application.Contracts.Users;
using SignalR.Contracts;

namespace SignalR.Hubs.Stream.Server.Abstractions;

public interface IStreamHubServerService
{
    Task OnStreamStartedAsync(GetStreamDto streamDto);
    Task OnStreamEndAsync(string streamerName);
    Task OnStreamChatOptionsChangedAsync(GetStreamOptionDto streamOptionDto, string streamerName);
    Task OnStreamChatMessageSendAsync(string streamerName, StreamChatMessageDto streamChatMessageDto);

    Task OnBlockFromStreamAsync(GetUserDto streamer, List<Guid> blockUserIds, bool isBlocked);

    Task OnUpsertModeratorsAsync(List<string> userIds);
}