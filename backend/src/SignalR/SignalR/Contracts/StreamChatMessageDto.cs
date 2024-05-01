using Application.Contracts.Users;

namespace SignalR.Contracts;

public readonly record struct StreamChatMessageDto(GetUserDto Sender, string Message);