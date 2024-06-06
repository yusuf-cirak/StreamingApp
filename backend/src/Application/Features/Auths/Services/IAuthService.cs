using Application.Contracts.Common.Dtos;
using Application.Contracts.Common.Models;

namespace Application.Features.Auths.Services;

public interface IAuthService : IDomainService<User>
{
    User RegisterUser(string username, string password, out AccessToken accessToken, out RefreshToken refreshToken,
        out Dictionary<string, object> claims);

    Task<GetUserRolesAndOperationClaimsDto> GetUserRolesAndOperationClaimsAsync(Guid userId,
        CancellationToken cancellationToken);
    
    GetUserRolesAndOperationClaimsDto GetUserRolesAndOperationClaims(User user);

    Task<List<GetUserRoleDto>> GetUserRolesAsync(Guid userId,
        CancellationToken cancellationToken);

    Task<List<GetUserOperationClaimDto>> GetUserOperationClaimsAsync(Guid userId,
        CancellationToken cancellationToken);
}