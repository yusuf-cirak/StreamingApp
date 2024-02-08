using Application.Features.StreamOptions.Dtos;
using Application.Features.Users.Dtos;

namespace Application.Features.Streams.Dtos;

public sealed record GetStreamDto(Guid Id, GetUserDto User, GetStreamOptionDto? StreamOption);