namespace Application.Features.Auths.Dtos;

public readonly record struct TokenResponseDto(string AccessToken, string RefreshToken);