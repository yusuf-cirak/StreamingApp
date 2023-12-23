using Application.Abstractions.Security;
using Application.Features.RoleOperationClaims.Rules;

namespace Application.Features.RoleOperationClaims.Commands.Delete;

[AuthorizationPipeline(Roles = ["Admin"])]
public readonly record struct DeleteRoleOperationClaimCommandRequest(Guid RoleId, Guid OperationClaimId)
    : IRequest<Result<bool, Error>>, ISecuredRequest;

public sealed class
    DeleteRoleOperationClaimCommandHandler : IRequestHandler<DeleteRoleOperationClaimCommandRequest,
    Result<bool, Error>>
{
    private readonly IEfRepository _efRepository;
    private readonly RoleOperationClaimBusinessRules _roleOperationClaimBusinessRules;

    public DeleteRoleOperationClaimCommandHandler(RoleOperationClaimBusinessRules roleOperationClaimBusinessRules,
        IEfRepository efRepository)
    {
        _roleOperationClaimBusinessRules = roleOperationClaimBusinessRules;
        _efRepository = efRepository;
    }

    public async Task<Result<bool, Error>> Handle(DeleteRoleOperationClaimCommandRequest request,
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

        var deleteResult = await _efRepository.RoleOperationClaims
            .Where(roc => roc.OperationClaimId == request.OperationClaimId &&
                          roc.RoleId == request.RoleId)
            .ExecuteDeleteAsync(cancellationToken);

        return deleteResult > 0;
    }
}