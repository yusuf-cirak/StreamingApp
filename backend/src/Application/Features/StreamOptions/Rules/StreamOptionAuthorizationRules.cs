using Application.Common.Errors;
using Application.Common.Extensions;
using Application.Features.StreamOptions.Abstractions;

namespace Application.Features.StreamOptions.Rules;

public static class StreamOptionAuthorizationRules
{
    public static Result UserMustBeStreamer(HttpContext context, ICollection<Claim> claims, object request)
    {
        // Check if user is the streamer
        if (IsUserStreamer(claims, request))
        {
            return Result.Success();
        }

        return Result.Failure(AuthorizationErrors.Unauthorized("User is not authorized to update stream"));
    }

    public static Result CanUserGetOrUpdateStreamOptions(HttpContext context, ICollection<Claim> claims, object request)
    {
        // Check if user is the streamer
        if (IsUserStreamer(claims, request))
        {
            return Result.Success();
        }

        // Check if user is a moderator of stream by role

        if (IsUserModeratorOfStreamByRole(claims, request))
        {
            return Result.Success();
        }
        // Check if user is a moderator of stream by operation claim

        if (IsUserModeratorOfStreamByOperationClaim(claims, request))
        {
            return Result.Success();
        }

        return Result.Failure(AuthorizationErrors.Unauthorized("User is not authorized"));
    }


    public static bool IsUserStreamer(ICollection<Claim> claims, object request)
    {
        Guid userId = Guid.Parse(claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ??
                                 string.Empty);

        Guid streamerId = ((IStreamOptionRequest)request).StreamerId;

        return userId == streamerId;
    }

    private static bool IsUserModeratorOfStreamByRole(ICollection<Claim> claims, object request)
    {
        string streamerIdString = ((IStreamOptionRequest)request).StreamerId.ToString();

        var roles = claims.GetRoles();

        return roles.Any(rc =>
            rc.Name == RoleConstants.StreamSuperModerator && rc.Value == streamerIdString);
    }

    private static bool IsUserModeratorOfStreamByOperationClaim(ICollection<Claim> claims, object request)
    {
        string streamerIdString = ((IStreamOptionRequest)request).StreamerId.ToString();

        var operationClaims = claims.GetOperationClaims();

        return operationClaims.Any(oc =>
            oc.Value == streamerIdString &&
            oc.Name == OperationClaimConstants.StreamUpdateTitleDescription);
    }
}