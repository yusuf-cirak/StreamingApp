using System.Security.Claims;
using Application.Common.Mapping;
using Application.Common.Rules;
using Application.Features.OperationClaims.Dtos;
using Application.Features.OperationClaims.Rules;

namespace Application.Features.OperationClaims.Commands.Update;

public readonly record struct UpdateOperationClaimCommandRequest
    : IRequest<HttpResult>, ISecuredRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public UpdateOperationClaimCommandRequest()
    {
        AuthorizationFunctions = [CommonAuthorizationRules.UserMustBeAdmin];
    }

    public UpdateOperationClaimCommandRequest(Guid id, string name) : this()
    {
        Id = id;
        Name = name;
    }
}

public sealed class
    UpdateOperationClaimCommandHandler : IRequestHandler<UpdateOperationClaimCommandRequest,
    HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

    public UpdateOperationClaimCommandHandler(OperationClaimBusinessRules operationClaimBusinessRules,
        IEfRepository efRepository)
    {
        _operationClaimBusinessRules = operationClaimBusinessRules;
        _efRepository = efRepository;
    }

    public async Task<HttpResult> Handle(UpdateOperationClaimCommandRequest request,
        CancellationToken cancellationToken)
    {
        var operationClaimResult = await _operationClaimBusinessRules.OperationClaimMustExistBeforeUpdated(request.Id);

        if (operationClaimResult.IsFailure)
        {
            return operationClaimResult.Error;
        }

        var operationClaim = operationClaimResult.Value;

        var duplicateResult =
            await _operationClaimBusinessRules.OperationClaimNameCannotBeDuplicatedBeforeRegistered(request.Name);

        if (duplicateResult.IsFailure)
        {
            return duplicateResult.Error;
        }

        operationClaim.Name = request.Name;

        _efRepository.OperationClaims.Update(operationClaim);

        var result = await _efRepository.SaveChangesAsync(cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : HttpResult.Failure(OperationClaimErrors.FailedToUpdate);
    }
}