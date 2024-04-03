using System.Security.Claims;
using Application.Common.Mapping;
using Application.Common.Rules;
using Application.Contracts.Roles;
using Application.Features.Roles.Rules;

namespace Application.Features.Roles.Commands.Create;

public readonly record struct CreateRoleCommandRequest
    : IRequest<HttpResult<GetRoleDto>>, ISecuredRequest
{
    public string Name { get; init; }
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public CreateRoleCommandRequest()
    {
        AuthorizationFunctions = [CommonAuthorizationRules.UserMustBeAdmin];
    }

    public CreateRoleCommandRequest(string name) : this()
    {
        Name = name;
    }
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