using Application.Contracts.StreamOptions;
using Application.Contracts.Users;

namespace Application.Contracts.Streamers;

public sealed record GetStreamerDto(GetUserDto User, GetStreamOptionDto StreamOption);