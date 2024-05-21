namespace Application.Contracts.Users;

public sealed record GetUserDto(Guid Id, string Username, string ProfileImageUrl);