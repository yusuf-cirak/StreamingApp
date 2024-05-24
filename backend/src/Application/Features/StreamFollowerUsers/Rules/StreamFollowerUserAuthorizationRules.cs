using Application.Common.Errors;
using Application.Features.StreamFollowerUsers.Abstractions;

namespace Application.Features.StreamFollowerUsers.Rules;

public static class StreamFollowerUserAuthorizationRules
{
    public static Result CanUserFollowStreamer(HttpContext context, ICollection<Claim> claims, object request)
    {
        var userId = Guid.Parse(claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty);

        if (userId == Guid.Empty)
        {
            return Result.Failure(AuthorizationErrors.Unauthorized());
        }

        var userIdFromRequest = ((IStreamFollowerUserCommandRequest)request).UserId;

        if (userId != userIdFromRequest)
        {
            return Result.Failure(AuthorizationErrors.Unauthorized());
        }

        return Result.Success();
    }
}