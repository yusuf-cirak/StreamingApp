using Application.Contracts.StreamOptions;
using Application.Contracts.Users;

namespace Application.Contracts.Streams;

public sealed record GetStreamDto(Guid Id, DateTime StartedAt, GetUserDto User, GetStreamOptionDto? StreamOption);