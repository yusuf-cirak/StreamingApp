namespace SignalR.Contracts;

public readonly record struct StreamBlockUserDto(Guid StreamerId, bool IsBlocked);