using System.Security.Claims;
using Application.Common.Rules;
using Application.Features.RoleOperationClaims.Rules;

namespace Application.Features.RoleOperationClaims.Commands.Delete;

public readonly record struct DeleteRoleOperationClaimCommandRequest
    : IRequest<HttpResult<bool>>, ISecuredRequest
{
    public Guid RoleId { get; init; }
    public Guid OperationClaimId { get; init; }
    public List<Func<ICollection<Claim>, object, Result>> AuthorizationRules { get; }
    
    public DeleteRoleOperationClaimCommandRequest()
    {
        AuthorizationRules = [CommonAuthorizationRules.UserMustBeAdmin];
    }
    
    public DeleteRoleOperationClaimCommandRequest(Guid roleId, Guid operationClaimId) : this()
    {
        RoleId = roleId;
        OperationClaimId = operationClaimId;
    }
}

public sealed class
    DeleteRoleOperationClaimCommandHandler : IRequestHandler<DeleteRoleOperationClaimCommandRequest,
    HttpResult<bool>>
{
    private readonly IEfRepository _efRepository;
    private readonly RoleOperationClaimBusinessRules _roleOperationClaimBusinessRules;

    public DeleteRoleOperationClaimCommandHandler(RoleOperationClaimBusinessRules roleOperationClaimBusinessRules,
        IEfRepository efRepository)
    {
        _roleOperationClaimBusinessRules = roleOperationClaimBusinessRules;
        _efRepository = efRepository;
    }

    public async Task<HttpResult<bool>> Handle(DeleteRoleOperationClaimCommandRequest request,
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

        await _efRepository.RoleOperationClaims
            .Where(roc => roc.OperationClaimId == request.OperationClaimId &&
                          roc.RoleId == request.RoleId)
            .ExecuteDeleteAsync(cancellationToken);

        return HttpResult<bool>.Success(true, StatusCodes.Status204NoContent);
    }
}