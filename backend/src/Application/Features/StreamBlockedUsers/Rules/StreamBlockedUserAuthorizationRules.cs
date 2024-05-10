using System.Text.Json;
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

        string rolesString = claims.First(c => c.Type == "Roles").Value;
        List<GetUserRoleDto> roleClaims = JsonSerializer.Deserialize<List<GetUserRoleDto>>(rolesString);

        return roleClaims.Exists(rc =>
            rc.Name == RoleConstants.StreamSuperModerator && rc.Value == streamerIdString);
    }

    private static bool IsUserModeratorOfStreamByOperationClaim(ICollection<Claim> claims, object request)
    {
        string streamerIdString = ((IStreamBlockedUserRequest)request).StreamerId.ToString();

        string operationClaimsString = claims.First(c => c.Type == "OperationClaims").Value;

        List<GetUserOperationClaimDto> operationClaims =
            JsonSerializer.Deserialize<List<GetUserOperationClaimDto>>(operationClaimsString);

        return operationClaims.Exists(oc =>
            oc.Value == streamerIdString &&
            oc.Name == OperationClaimConstants.StreamBlockUserFromChat);
    }
}