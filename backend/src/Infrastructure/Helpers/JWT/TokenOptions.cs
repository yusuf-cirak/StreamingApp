namespace Infrastructure.Helpers.JWT;

public sealed class TokenOptions
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public int AccessTokenExpiration { get; set; }
    public string SecurityKey { get; set; }

    public TokenOptions()
    {
    }
    
    public TokenOptions(string audience, string issuer, int accessTokenExpiration, string securityKey)
    {
        Audience = audience;
        Issuer = issuer;
        AccessTokenExpiration = accessTokenExpiration;
        SecurityKey = securityKey;
    }
}