using Application.Features.Streamers.Dtos;
using Application.Features.Users.Dtos;

namespace Application.Features.Streams.Dtos;

public record GetStreamDto(Guid Id, DateTime StartedAt, GetUserDto User, GetStreamerDto Streamer);