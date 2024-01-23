using System.Text.Json;
using Application.Common.Errors;
using Application.Features.OperationClaims.Dtos;
using Application.Features.Roles.Dtos;
using Application.Features.StreamOptions.Abstractions;

namespace Application.Features.StreamOptions.Rules;

public static class StreamOptionAuthorizationRules
{
    public static Result CanUserUpdateStreamer(HttpContext context, ICollection<Claim> claims, object request)
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

        return Result.Failure(AuthorizationErrors.Unauthorized("User is not authorized to update stream"));
    }


    private static bool IsUserStreamer(ICollection<Claim> claims, object request)
    {
        Guid userId = Guid.Parse(claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ??
                                 string.Empty);

        Guid streamerId = ((IStreamOptionCommandRequest)request).StreamerId;

        return userId == streamerId;
    }

    private static bool IsUserModeratorOfStreamByRole(ICollection<Claim> claims, object request)
    {
        string streamerIdString = ((IStreamOptionCommandRequest)request).StreamerId.ToString();

        string rolesString = claims.First(c => c.Type == "Roles").Value;
        List<GetUserRoleDto> roleClaims = JsonSerializer.Deserialize<List<GetUserRoleDto>>(rolesString);

        return roleClaims.Exists(rc =>
            rc.Role.Name == RoleConstants.StreamSuperModerator && rc.Value == streamerIdString);
    }

    private static bool IsUserModeratorOfStreamByOperationClaim(ICollection<Claim> claims, object request)
    {
        string streamerIdString = ((IStreamOptionCommandRequest)request).StreamerId.ToString();

        string operationClaimsString = claims.First(c => c.Type == "OperationClaims").Value;

        List<GetUserOperationClaimDto> operationClaims =
            JsonSerializer.Deserialize<List<GetUserOperationClaimDto>>(operationClaimsString);

        return operationClaims.Exists(oc =>
            oc.Value == streamerIdString &&
            oc.OperationClaim.Name == OperationClaimConstants.StreamUpdateTitleDescription);
    }
}