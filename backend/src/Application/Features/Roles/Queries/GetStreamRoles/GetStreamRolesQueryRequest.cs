using Application.Common.Permissions;
using Application.Features.Roles.Services;

namespace Application.Features.Roles.Queries.GetStreamRoles;

public readonly record struct GetStreamRolesQueryRequest() : IRequest<IEnumerable<GetRoleDto>>, ISecuredRequest
{
    public PermissionRequirements PermissionRequirements { get; } = PermissionRequirements.Create();
}

public sealed class GetAllRolesQueryHandler(IRoleService roleService)
    : IRequestHandler<GetStreamRolesQueryRequest, IEnumerable<GetRoleDto>>
{
    public Task<IEnumerable<GetRoleDto>> Handle(GetStreamRolesQueryRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(roleService.GetStreamRoles());
    }
}