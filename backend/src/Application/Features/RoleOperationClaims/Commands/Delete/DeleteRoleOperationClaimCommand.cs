using Application.Abstractions.Security;
using Application.Features.RoleOperationClaims.Rules;
using Microsoft.AspNetCore.Http;

namespace Application.Features.RoleOperationClaims.Commands.Delete;

[AuthorizationPipeline(Roles = ["Admin"])]
public readonly record struct DeleteRoleOperationClaimCommandRequest(Guid RoleId, Guid OperationClaimId)
    : IRequest<HttpResult<bool, Error>>, ISecuredRequest;

public sealed class
    DeleteRoleOperationClaimCommandHandler : IRequestHandler<DeleteRoleOperationClaimCommandRequest,
    HttpResult<bool, Error>>
{
    private readonly IEfRepository _efRepository;
    private readonly RoleOperationClaimBusinessRules _roleOperationClaimBusinessRules;

    public DeleteRoleOperationClaimCommandHandler(RoleOperationClaimBusinessRules roleOperationClaimBusinessRules,
        IEfRepository efRepository)
    {
        _roleOperationClaimBusinessRules = roleOperationClaimBusinessRules;
        _efRepository = efRepository;
    }

    public async Task<HttpResult<bool, Error>> Handle(DeleteRoleOperationClaimCommandRequest request,
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

        return HttpResult<bool, Error>.Success(true, StatusCodes.Status204NoContent);
    }
}