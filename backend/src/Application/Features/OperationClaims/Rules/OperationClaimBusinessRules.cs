using Application.Abstractions;

namespace Application.Features.OperationClaims.Rules;

public sealed class OperationClaimBusinessRules : BaseBusinessRules
{
    private readonly IEfRepository _efRepository;

    public OperationClaimBusinessRules(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<Result> OperationClaimNameCannotBeDuplicatedBeforeRegistered(string name)
    {
        var operationClaim = await _efRepository.OperationClaims.SingleOrDefaultAsync(oc => oc.Name == name);

        if (operationClaim is not null)
        {
            return Result.Failure(OperationClaimErrors.NameCannotBeDuplicated);
        }

        return Result.Success();
    }

    public async Task<Result<OperationClaim, Error>> OperationClaimMustExistBeforeUpdated(Guid operationClaimId)
    {
        var operationClaim = await _efRepository.OperationClaims.SingleOrDefaultAsync(oc => oc.Id == operationClaimId);

        if (operationClaim is null)
        {
            return OperationClaimErrors.NotFound;
        }

        return operationClaim;
    }
}