using Application.Common.Permissions;
using Application.Features.Roles.Rules;

namespace Application.Features.Roles.Commands.Delete;

public readonly record struct DeleteRoleCommandRequest() : IRequest<HttpResult>, IPermissionRequest
{
    public Guid Id { get; init; }

    public PermissionRequirements PermissionRequirements { get; } = PermissionRequirementConstants.WithAdminRole();
}

public sealed class DeleteRoleCommandHandler(IEfRepository efRepository, RoleBusinessRules roleBusinessRules)
    : IRequestHandler<DeleteRoleCommandRequest, HttpResult>
{
    public async Task<HttpResult> Handle(DeleteRoleCommandRequest request,
        CancellationToken cancellationToken)
    {
        var roleExistResult = await roleBusinessRules.RoleMustExistBeforeDeleted(request.Id, cancellationToken);

        if (roleExistResult.IsFailure)
        {
            return roleExistResult.Error;
        }

        var result = await efRepository.Roles.Where(r => r.Id == request.Id).ExecuteDeleteAsync(cancellationToken);


        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : RoleErrors.FailedToDelete;
    }
}