using Application.Contracts.StreamOptions;
using Application.Contracts.Users;

namespace Application.Contracts.Streams;

public sealed record GetFollowingStreamDto(GetUserDto User,GetStreamOptionDto StreamOption);