using Application.Features.StreamOptions.Dtos;
using Application.Features.Users.Dtos;

namespace Application.Features.Streams.Dtos;

public record GetStreamDto(Guid Id, DateTime StartedAt, GetUserDto User, GetStreamOptionDto StreamOption);