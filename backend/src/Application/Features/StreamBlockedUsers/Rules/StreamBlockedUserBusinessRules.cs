using System.Security.Claims;
using Application.Abstractions;
using Application.Common.Extensions;

namespace Application.Features.StreamBlockedUsers.Rules;

public sealed class StreamBlockedUserBusinessRules : BaseBusinessRules
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StreamBlockedUserBusinessRules(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> BlockerUserShouldBeAdminOrModerator(Guid userId, Guid streamerId)
    {
        // TODO: Read user claims with a better way
        List<string> userClaims =
            _httpContextAccessor?.HttpContext?.User.Claims(OperationClaimConstants.StreamBlockUserFromChat) ?? new();

        if (userClaims.Count == 0)
        {
            return Result.Failure(StreamErrors.UserIsNotModeratorOfStream);
        }

        // TODO: Read user claims
        if (!userClaims.Exists(uc => uc == streamerId.ToString()))
        {
            return Result.Failure(StreamErrors.UserIsNotModeratorOfStream);
        }

        // var streamModeratorRole =
        //     await _efRepository
        //         .UserRoleClaims
        //         .Include(urc => urc.Role)
        //         .SingleOrDefaultAsync(urc =>
        //             urc.UserId == userId && urc.Role.Name == RoleConstants.StreamSuperModerator &&
        //             urc.Value == streamerId.ToString());
        //
        // if (streamModeratorRole is not null)
        // {
        //     return Result.Success();
        // }
        //
        // var streamModeratorClaim = await _efRepository
        //     .UserOperationClaims
        //     .Include(uoc => uoc.OperationClaim)
        //     .SingleOrDefaultAsync(urc =>
        //         urc.UserId == userId &&
        //         urc.OperationClaim.Name == OperationClaimConstants.StreamBlockUserFromChat &&
        //         urc.Value == streamerId.ToString());
        //
        // if (streamModeratorClaim is not null)
        // {
        //     return Result.Success();
        // }

        return Result.Failure(StreamErrors.UserIsNotModeratorOfStream);
    }
}