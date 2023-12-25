using Application.Features.Roles.Rules;

namespace Application.Features.Roles.Commands.Delete;

[AuthorizationPipeline(Roles = ["Admin"])]
public readonly record struct DeleteRoleCommandRequest(Guid Id)
    : IRequest<HttpResult<bool>>;

public sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommandRequest, HttpResult<bool>>
{
    private readonly IEfRepository _efRepository;
    private readonly RoleBusinessRules _roleBusinessRules;

    public DeleteRoleCommandHandler(IEfRepository efRepository, RoleBusinessRules roleBusinessRules)
    {
        _efRepository = efRepository;
        _roleBusinessRules = roleBusinessRules;
    }

    public async Task<HttpResult<bool>> Handle(DeleteRoleCommandRequest request,
        CancellationToken cancellationToken)
    {
        var roleExistResult = await _roleBusinessRules.RoleMustExistBeforeDeleted(request.Id, cancellationToken);

        if (roleExistResult.IsFailure)
        {
            return roleExistResult.Error;
        }

        await _efRepository.Roles.Where(r => r.Id == request.Id).ExecuteDeleteAsync(cancellationToken);

        return HttpResult<bool>.Success(true, StatusCodes.Status204NoContent);
    }
}