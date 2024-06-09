using Application.Common.Permissions;
using Application.Features.OperationClaims.Services;

namespace Application.Features.OperationClaims.Queries.GetAll;

public readonly record struct GetStreamOperationClaimsQueryRequest()
    : IRequest<IEnumerable<GetOperationClaimDto>>, ISecuredRequest;

public sealed class GetAllOperationClaimsQueryRequestHandler(IOperationClaimService operationClaimService)
    : IRequestHandler<GetStreamOperationClaimsQueryRequest, IEnumerable<GetOperationClaimDto>>
{
    public Task<IEnumerable<GetOperationClaimDto>> Handle(GetStreamOperationClaimsQueryRequest request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(operationClaimService.GetStreamOperationClaims());
    }
}