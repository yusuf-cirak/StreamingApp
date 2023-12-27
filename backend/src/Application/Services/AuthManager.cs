using Application.Common.Dtos;
using Application.Common.Dtos.Authorization;
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

    public async Task<UserAuthorizationDto> GetUserAuthorizations(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userRoleClaims = GetUserRoles(userId, cancellationToken);
        var userOperationClaims = GetUserOperationClaims(userId, cancellationToken);


        await Task.WhenAll(userRoleClaims, userOperationClaims);

        var systemAuthorizations = GetSystemAuthorizations(userRoleClaims.Result, userOperationClaims.Result);

        // return new UserAuthorizationDto(userId, systemAuthorizations, userRoleClaims.Result,
        //     userOperationClaims.Result);
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

    private async Task<List<GetRoleDto>> GetUserRoles(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _efRepository.UserRoleClaims
            .Include(urc => urc.Role)
            .Where(urc => urc.UserId == userId)
            .Select(urc => new GetRoleDto(urc.Role.Id, urc.Role.Name))
            .ToListAsync(cancellationToken);
    }

    private async Task<List<GetOperationClaimDto>> GetUserOperationClaims(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _efRepository.UserOperationClaims
            .Include(uoc => uoc.OperationClaim)
            .Where(uoc => uoc.UserId == userId)
            .Select(uoc => new GetOperationClaimDto(uoc.OperationClaim.Id, uoc.OperationClaim.Name))
            .ToListAsync(cancellationToken);
    }

    private StreamAuthorization GetSystemAuthorizations(List<GetRoleDto> userRoles,
        List<GetOperationClaimDto> userOperationClaims)
    {
        var systemRoles = GetSystemRoles(userRoles);
        var systemOperationClaims = GetSystemOperationClaims(userOperationClaims);

        return new StreamAuthorization(systemRoles, systemOperationClaims);
    }

    private List<GetRoleDto> GetSystemRoles(List<GetRoleDto> roles)
    {
        return roles.Where(role => role.Name.Contains(RoleConstants.System)).ToList();
    }

    private List<GetOperationClaimDto> GetSystemOperationClaims(List<GetOperationClaimDto> operationClaims)
    {
        return operationClaims.Where(operationClaim => operationClaim.Name.Contains(OperationClaimConstants.System))
            .ToList();
    }
}