namespace Application.Features.Streams.Dtos;

public sealed record GetStreamBlockedUserDto(Guid Id, string Username, string ProfileImageUrl, DateTime BlockedAt);