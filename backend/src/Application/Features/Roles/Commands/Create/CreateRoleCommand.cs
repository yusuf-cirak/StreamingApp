using Application.Common.Mapping;
using Application.Features.Roles.Dtos;
using Application.Features.Roles.Rules;

namespace Application.Features.Roles.Commands.Create;

public readonly record struct CreateRoleCommandRequest(string Name)
    : IRequest<HttpResult<GetRoleDto>>;

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

        await _efRepository.SaveChangesAsync(cancellationToken);

        var roleDto = role.ToDto();

        return HttpResult<GetRoleDto>.Success(roleDto, StatusCodes.Status201Created);
    }
}