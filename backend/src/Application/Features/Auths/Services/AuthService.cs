namespace Application.Features.Auths.Services;

public sealed class AuthService : IAuthService
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetUserRolesAndOperationClaimsDto> GetUserRolesAndOperationClaimsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userRoles = await GetUserRolesAsync(userId, cancellationToken);
        var userOperationClaims = await GetUserOperationClaimsAsync(userId, cancellationToken);

        return new GetUserRolesAndOperationClaimsDto(userId, userRoles, userOperationClaims);
    }

    public Task<List<GetUserRoleDto>> GetUserRolesAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return _efRepository
            .UserRoleClaims
            .Include(uoc => uoc.Role)
            .Where(uoc => uoc.UserId == userId)
            .Select(uoc => new GetUserRoleDto()
            {
                Name = uoc.Role.Name,
                Value = uoc.Value
            }).ToListAsync(cancellationToken);
    }

    public Task<List<GetUserOperationClaimDto>> GetUserOperationClaimsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return _efRepository
            .UserOperationClaims
            .Include(uoc => uoc.OperationClaim)
            .Where(uoc => uoc.UserId == userId)
            .Select(uoc => new GetUserOperationClaimDto()
            {
                Name = uoc.OperationClaim.Name,
                Value = uoc.Value
            })
            .ToListAsync(cancellationToken);
    }
}