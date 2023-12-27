using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Application.Abstractions.Helpers;
using Application.Common.Models;
using Infrastructure.Helpers.Security.Encryption;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Infrastructure.Helpers.JWT;

public sealed class JwtHelper : IJwtHelper
{
    private readonly TokenOptions _tokenOptions;

    private DateTime _accessTokenExpiration;

    public JwtHelper(IOptions<TokenOptions> tokenOptions)
    {
        _tokenOptions = tokenOptions.Value;
    }


    public AccessToken CreateAccessToken(User user, Dictionary<string, object> claimsDictionary)
    {
        _accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpiration);
        SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
        SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
        JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, claimsDictionary);
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        string token = jwtSecurityTokenHandler.WriteToken(jwt);
        return new AccessToken(token, _accessTokenExpiration);
    }

    private JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
        SigningCredentials signingCredentials, Dictionary<string, object> claimsDictionary)
    {
        JwtSecurityToken jwt = new(
            tokenOptions.Issuer,
            tokenOptions.Audience,
            expires: _accessTokenExpiration,
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials,
            claims: SetClaims(user, claimsDictionary)
        );
        return jwt;
    }

    private IEnumerable<Claim> SetClaims(User user, Dictionary<string, object> claimsDictionary)
    {
        List<Claim> claims = new(3)
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()!),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("ProfileImageUrl", user.ProfileImageUrl),
        };

        foreach (var (key, value) in claimsDictionary)
        {
            string serializedValue = JsonConvert.SerializeObject(value, Formatting.None);
            claims.Add(new Claim(key, serializedValue));
        }

        return claims;
    }

    public RefreshToken CreateRefreshToken(User user, string ipAddress)
    {
        var secureRandomBytes = new byte[64];

        using var randomNumberGenerator = RandomNumberGenerator.Create();

        randomNumberGenerator.GetBytes(secureRandomBytes);

        return RefreshToken.Create(Convert.ToBase64String(secureRandomBytes), user.Id, ipAddress,
            _accessTokenExpiration.AddDays(7));
    }
}