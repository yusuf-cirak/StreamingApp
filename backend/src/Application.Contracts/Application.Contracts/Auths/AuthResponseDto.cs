namespace Application.Contracts.Auths;

public readonly record struct AuthResponseDto(Guid Id, string Username,string ProfileImageUrl,string AccessToken, string RefreshToken,DateTime TokenExpiration,Dictionary<string,dynamic> claims);