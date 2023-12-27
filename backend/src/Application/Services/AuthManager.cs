using Application.Common.Dtos;
using Application.Features.OperationClaims.Dtos;
using Application.Features.Roles.Dtos;

namespace Application.Services;

public sealed class AuthManager
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthManager(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetUserRolesAndOperationClaimsDto> GetUserRolesAndOperationClaimsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userRoles = GetUserRolesAsync(userId, cancellationToken);
        var userOperationClaims = GetUserOperationClaimsAsync(userId, cancellationToken);

        await Task.WhenAll(new List<Task>(2) { userRoles, userOperationClaims });

        return new GetUserRolesAndOperationClaimsDto(userId, userRoles.Result, userOperationClaims.Result);
    }

    public async Task<List<GetUserRoleDto>> GetUserRolesAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userRoles = await _efRepository
            .UserRoleClaims
            .Include(uoc => uoc.Role)
            .Where(uoc => uoc.UserId == userId)
            .Select(uoc => new GetUserRoleDto()
            {
                Role = new GetRoleDto(uoc.Role.Id, uoc.Role.Name),
                Value = uoc.Value
            })
            .ToListAsync(cancellationToken);

        return userRoles;
    }

    public async Task<List<GetUserOperationClaimDto>> GetUserOperationClaimsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userOperationClaims = await _efRepository
            .UserOperationClaims
            .Include(uoc => uoc.OperationClaim)
            .Where(uoc => uoc.UserId == userId)
            .Select(uoc => new GetUserOperationClaimDto()
            {
                OperationClaim = new GetOperationClaimDto(uoc.OperationClaim.Id, uoc.OperationClaim.Name),
                Value = uoc.Value
            })
            .ToListAsync(cancellationToken);

        return userOperationClaims;
    }


    public bool IsAdmin(Guid userId, List<GetRoleDto> userRoles)
    {
        return userRoles.Exists(ur => ur.Name == "Admin");
    }

    public bool IsModerator(Guid userId, List<GetRoleDto> userRoles)
    {
        return userRoles.Exists(ur => ur.Name == "Moderator");
    }

    public bool IsStreamer(Guid userId, List<GetRoleDto> userRoles)
    {
        return userRoles.Exists(ur => ur.Name == "Streamer");
    }

    public bool IsUser(Guid userId, List<GetRoleDto> userRoles)
    {
        return userRoles.Exists(ur => ur.Name == "User");
    }

    public bool IsBlockedFromStream(Guid userId, List<GetRoleDto> userRoles)
    {
        return userRoles.Exists(ur => ur.Name == "Blocked");
    }
}