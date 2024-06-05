namespace Application.Features.OperationClaims.Services;

public interface IOperationClaimService : IDomainService<OperationClaim>
{
    IEnumerable<GetOperationClaimDto> GetStreamOperationClaims();
}

public sealed class OperationClaimService(IEfRepository efRepository) : IOperationClaimService
{
    public IEnumerable<GetOperationClaimDto> GetStreamOperationClaims()
    {
        return efRepository.OperationClaims
            .Where(o => EF.Functions.Like(o.Name, "%Stream%"))
            .Select(oc => new GetOperationClaimDto(oc.Id, oc.Name));
    }
}