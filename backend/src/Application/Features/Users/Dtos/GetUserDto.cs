namespace Application.Features.Users.Dtos;

public readonly record struct GetUserDto(Guid Id, string Username, string ProfileImageUrl);