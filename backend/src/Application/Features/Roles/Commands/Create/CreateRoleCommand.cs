using Application.Common.Mapping;
using Application.Common.Permissions;
using Application.Features.Roles.Rules;

namespace Application.Features.Roles.Commands.Create;

public readonly record struct CreateRoleCommandRequest() : IRequest<HttpResult<GetRoleDto>>, IPermissionRequest
{
    public string Name { get; init; }
    public PermissionRequirements PermissionRequirements { get; } = PermissionRequirementConstants.WithAdminRole();
}

public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommandRequest, HttpResult<GetRoleDto>>
{
    private readonly IEfRepository _efRepository;
    private readonly RoleBusinessRules _roleBusinessRules;

    public CreateRoleCommandHandler(IEfRepository efRepository, RoleBusinessRules roleBusinessRules)
    {
        _efRepository = efRepository;
        _roleBusinessRules = roleBusinessRules;
    }

    public async Task<HttpResult<GetRoleDto>> Handle(CreateRoleCommandRequest request,
        CancellationToken cancellationToken)
    {
        var uniqueRoleNameResult = await _roleBusinessRules.RoleNameMustBeUnique(request.Name, cancellationToken);

        if (uniqueRoleNameResult.IsFailure)
        {
            return uniqueRoleNameResult.Error;
        }

        var role = Role.Create(request.Name);

        _efRepository.Roles.Add(role);

        var result = await _efRepository.SaveChangesAsync(cancellationToken);

        return result > 0
            ? HttpResult<GetRoleDto>.Success(role.ToDto(), StatusCodes.Status201Created)
            : RoleErrors.FailedToCreate;
    }
}