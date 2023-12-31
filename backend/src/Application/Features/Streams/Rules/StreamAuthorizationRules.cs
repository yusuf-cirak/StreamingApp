using System.Text.Json;
using Application.Features.OperationClaims.Dtos;
using Application.Features.Roles.Dtos;
using Application.Features.StreamBlockedUsers.Abstractions;
using Application.Features.Streamers.Abstractions;

namespace Application.Features.Streams.Rules;

public static class StreamAuthorizationRules
{
    public static Result CanRequestorCreateOrUpdateStream(ICollection<Claim> claims, object request)
    {
        // Check if user is the streamer
        if (IsUserStreamer(claims, request))
        {
            return Result.Success();
        }

        return Result.Failure(StreamErrors.UserIsNotStreamer);
    }

    private static bool IsUserStreamer(ICollection<Claim> claims, object request)
    {
        Guid userId = Guid.Parse(claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ??
                                 string.Empty);

        Guid streamerId = ((IStreamerCommandRequest)request).StreamerId;

        return userId == streamerId;
    }

    private static bool IsUserModeratorOfStreamByRole(ICollection<Claim> claims, object request)
    {
        string streamerIdString = ((IStreamerCommandRequest)request).StreamerId.ToString();

        string rolesString = claims.First(c => c.Type == "Roles").Value;
        List<GetUserRoleDto> roleClaims = JsonSerializer.Deserialize<List<GetUserRoleDto>>(rolesString);

        return roleClaims.Exists(rc =>
            rc.Role.Name == RoleConstants.StreamSuperModerator && rc.Value == streamerIdString);
    }

    private static bool IsUserModeratorOfStreamByOperationClaim(ICollection<Claim> claims, object request)
    {
        string streamerIdString = ((IStreamBlockedUserRequest)request).StreamerId.ToString();

        string operationClaimsString = claims.First(c => c.Type == "OperationClaims").Value;

        List<GetUserOperationClaimDto> operationClaims =
            JsonSerializer.Deserialize<List<GetUserOperationClaimDto>>(operationClaimsString);

        return operationClaims.Exists(oc =>
            oc.Value == streamerIdString &&
            oc.OperationClaim.Name == OperationClaimConstants.StreamBlockUserFromChat);
    }
}