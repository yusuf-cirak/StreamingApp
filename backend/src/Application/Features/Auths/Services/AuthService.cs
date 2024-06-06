using Application.Common.Mapping;
using Application.Contracts.Common.Dtos;
using Application.Contracts.Common.Models;

namespace Application.Features.Auths.Services;

public sealed class AuthService : IAuthService
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHashingHelper _hashingHelper;
    private readonly IJwtHelper _jwtHelper;

    public AuthService(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor,
        IHashingHelper hashingHelper, IJwtHelper jwtHelper)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
        _hashingHelper = hashingHelper;
        _jwtHelper = jwtHelper;
    }

    public User RegisterUser(string username, string password, out AccessToken accessToken,
        out RefreshToken refreshToken, out Dictionary<string, object> claims)
    {
        _hashingHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        User user = User.Create(username, passwordHash, passwordSalt);


        var userIpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();


        claims = new()
        {
            ["roles"] = Enumerable.Empty<GetUserRoleDto>(),
            ["operationClaims"] = Enumerable.Empty<GetUserOperationClaimDto>()
        };

        accessToken = _jwtHelper.CreateAccessToken(user, claims);
        refreshToken = _jwtHelper.CreateRefreshToken(user, userIpAddress);

        _efRepository.Users.Add(user);
        _efRepository.RefreshTokens.Add(refreshToken);

        return user;
    }

    public async Task<GetUserRolesAndOperationClaimsDto> GetUserRolesAndOperationClaimsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userRoles = await GetUserRolesAsync(userId, cancellationToken);
        var userOperationClaims = await GetUserOperationClaimsAsync(userId, cancellationToken);

        return new GetUserRolesAndOperationClaimsDto(userId, userRoles, userOperationClaims);
    }

    public GetUserRolesAndOperationClaimsDto GetUserRolesAndOperationClaims(User user)
    {
        var roles = user
            .UserRoleClaims
            .Select(urc => new GetUserRoleDto(urc.Role.Name, urc.Value));

        var operationClaims = user
            .UserOperationClaims
            .Select(uoc => new GetUserOperationClaimDto(uoc.OperationClaim.Name, uoc.Value));
        
        return new GetUserRolesAndOperationClaimsDto(user.Id,roles,operationClaims);

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