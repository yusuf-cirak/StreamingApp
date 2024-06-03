using Application.Common.Permissions;
using Application.Features.RoleOperationClaims.Rules;

namespace Application.Features.RoleOperationClaims.Commands.Delete;

public readonly record struct DeleteRoleOperationClaimCommandRequest() : IRequest<HttpResult>, ISecuredRequest
{
    public Guid RoleId { get; init; }
    public Guid OperationClaimId { get; init; }
    public PermissionRequirements PermissionRequirements { get; } = PermissionRequirementConstants.WithAdminRole();
}

public sealed class
    DeleteRoleOperationClaimCommandHandler : IRequestHandler<DeleteRoleOperationClaimCommandRequest,
    HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly RoleOperationClaimBusinessRules _roleOperationClaimBusinessRules;

    public DeleteRoleOperationClaimCommandHandler(RoleOperationClaimBusinessRules roleOperationClaimBusinessRules,
        IEfRepository efRepository)
    {
        _roleOperationClaimBusinessRules = roleOperationClaimBusinessRules;
        _efRepository = efRepository;
    }

    public async Task<HttpResult> Handle(DeleteRoleOperationClaimCommandRequest request,
        CancellationToken cancellationToken)
    {
        var existResult =
            await _roleOperationClaimBusinessRules.RoleOperationClaimMustExistBeforeDeleted(
                request.RoleId,
                request.OperationClaimId);

        if (existResult.IsFailure)
        {
            return existResult.Error;
        }

        var result = await _efRepository.RoleOperationClaims
            .Where(roc => roc.OperationClaimId == request.OperationClaimId &&
                          roc.RoleId == request.RoleId)
            .ExecuteDeleteAsync(cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : RoleOperationClaimErrors.CouldNotBeDeleted;
    }
}