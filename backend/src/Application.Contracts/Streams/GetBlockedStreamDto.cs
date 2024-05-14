using Application.Contracts.Users;

namespace Application.Contracts.Streams;

public sealed record GetBlockedStreamDto(GetUserDto User);