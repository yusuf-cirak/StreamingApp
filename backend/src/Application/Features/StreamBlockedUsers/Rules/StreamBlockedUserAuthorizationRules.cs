using Application.Common.Extensions;
using Application.Features.StreamBlockedUsers.Abstractions;

namespace Application.Features.StreamBlockedUsers.Rules;

public static class StreamBlockedUserAuthorizationRules
{
    public static Result CanUserBlockOrUnblockAUserFromStream(HttpContext context, ICollection<Claim> claims,
        object request)
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

        return Result.Failure(StreamErrors.UserIsNotModeratorOfStream);
    }

    private static bool IsUserStreamer(ICollection<Claim> claims, object request)
    {
        Guid userId = Guid.Parse(claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ??
                                 string.Empty);

        Guid streamerId = ((IStreamBlockedUserRequest)request).StreamerId;

        return userId == streamerId;
    }

    private static bool IsUserModeratorOfStreamByRole(ICollection<Claim> claims, object request)
    {
        string streamerIdString = ((IStreamBlockedUserRequest)request).StreamerId.ToString();

        var roles = claims.GetRoles();

        return roles.Any(rc =>
            rc.Name == RoleConstants.StreamSuperModerator ||
            rc.Name == RoleConstants.StreamModerator && rc.Value == streamerIdString);
    }

    private static bool IsUserModeratorOfStreamByOperationClaim(ICollection<Claim> claims, object request)
    {
        string streamerIdString = ((IStreamBlockedUserRequest)request).StreamerId.ToString();

        var operationClaims = claims.GetOperationClaims();

        return operationClaims.Any(oc =>
            oc.Value == streamerIdString &&
            oc.Name == OperationClaimConstants.Stream.Write.BlockFromChat);
    }
}