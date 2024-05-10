namespace Application.Features.Auths.Services;

public interface IAuthService : IDomainService<User>
{
    Task<GetUserRolesAndOperationClaimsDto> GetUserRolesAndOperationClaimsAsync(Guid userId,
        CancellationToken cancellationToken);

    Task<List<GetUserRoleDto>> GetUserRolesAsync(Guid userId,
        CancellationToken cancellationToken);

    Task<List<GetUserOperationClaimDto>> GetUserOperationClaimsAsync(Guid userId,
        CancellationToken cancellationToken);
}