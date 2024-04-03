namespace Application.Contracts.Users;

public readonly record struct GetUserDto(Guid Id, string Username, string ProfileImageUrl);