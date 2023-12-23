using Application.Abstractions;

namespace Application.Features.RoleOperationClaims.Rules;

public sealed class RoleOperationClaimBusinessRules : BaseBusinessRules
{
    private readonly IEfRepository _efRepository;

    public RoleOperationClaimBusinessRules(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<Result> RoleOperationClaimMustNotExistBeforeRegistered(Guid roleId, Guid operationClaimId)
    {
        var roleOperationClaim = await _efRepository.RoleOperationClaims.SingleOrDefaultAsync(roc =>
            roc.RoleId == roleId && roc.OperationClaimId == operationClaimId);

        if (roleOperationClaim is not null)
        {
            return Result.Failure(RoleOperationClaimErrors.OperationClaimIdCannotBeDuplicated);
        }

        return Result.Success();
    }

    public async Task<Result> RoleOperationClaimMustExistBeforeDeleted(Guid roleId,
        Guid operationClaimId)
    {
        var roleOperationClaim = await _efRepository.RoleOperationClaims.SingleOrDefaultAsync(roc =>
            roc.RoleId == roleId && roc.OperationClaimId == operationClaimId);

        if (roleOperationClaim is null)
        {
            return Result.Failure(RoleOperationClaimErrors.OperationClaimDoesNotExist);
        }

        return Result.Success();
    }
}