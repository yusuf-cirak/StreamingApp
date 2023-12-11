using Infrastructure.Helpers.JWT;
using Infrastructure.Helpers.Security.Encryption;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebAPI.Extensions;

internal static class JwtBearerExtensions
{
    internal static void AddJwtAuthenticationServices(this IServiceCollection services,IConfiguration configuration)
    {
        TokenOptions tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>()!;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt=>
        {
            opt.TokenValidationParameters = new()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = tokenOptions.Audience,
                ValidIssuer = tokenOptions.Issuer,
                IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
                ClockSkew = TimeSpan.Zero
            };
            
            opt.Audience = tokenOptions.Audience;
        });
    }
}