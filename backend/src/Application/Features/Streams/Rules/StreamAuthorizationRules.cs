using Application.Common.Errors;
using Application.Common.Extensions;
using Application.Features.StreamBlockedUsers.Abstractions;
using Application.Features.Streams.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Streams.Rules;

public static class StreamAuthorizationRules
{
    private const string HeaderApiKey = "X-Api-Key";

    public static Result RequesterMustHaveValidApiKey(HttpContext context, ICollection<Claim> claims, object request)
    {
        var apiKeyResult = GetApiKeyFromRequest(context);

        if (apiKeyResult.IsFailure)
        {
            Result.Failure(apiKeyResult.Error);
        }


        IConfiguration configuration = context.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;


        return IsRequesterAuthorized(configuration, apiKeyResult.Value);
    }

    private static Result<string, Error> GetApiKeyFromRequest(HttpContext context)
    {
        var apiKey = context.Request.Headers[HeaderApiKey].FirstOrDefault();

        if (string.IsNullOrEmpty(apiKey))
        {
            return (AuthorizationErrors.Unauthorized());
        }

        return apiKey;
    }

    private static Result IsRequesterAuthorized(IConfiguration configuration, string apiKey)
    {
        var isAuthorized = configuration.GetValue<string>("ApiKey").Equals(apiKey);

        return !isAuthorized
            ? Result.Failure(AuthorizationErrors.Unauthorized())
            : Result.Success();
    }


}