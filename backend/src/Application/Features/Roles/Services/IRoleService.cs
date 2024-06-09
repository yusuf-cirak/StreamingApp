namespace Application.Features.Roles.Services;

public interface IRoleService : IDomainService<Role>
{
    IEnumerable<GetRoleDto> GetStreamRoles();
}

public sealed class RoleService(IEfRepository efRepository) : IRoleService
{
    public IEnumerable<GetRoleDto> GetStreamRoles()
    {
        return efRepository.Roles
            .Where(r => EF.Functions.Like(r.Name, "%Stream%"))
            .Select(r => new GetRoleDto(r.Id, r.Name));
    }
}