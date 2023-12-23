using Application.Abstractions.Security;
using Application.Common.Mapping;
using Application.Features.OperationClaims.Dtos;
using Application.Features.OperationClaims.Rules;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.OperationClaims.Commands.Update;

public readonly record struct UpdateOperationClaimCommandRequest(Guid Id, string Name)
    : IRequest<Result<GetOperationClaimDto, Error>>, ISecuredRequest;

public sealed class
    UpdateOperationClaimCommandHandler : IRequestHandler<UpdateOperationClaimCommandRequest,
    Result<GetOperationClaimDto, Error>>
{
    private readonly IEfRepository _efRepository;
    private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

    public UpdateOperationClaimCommandHandler(OperationClaimBusinessRules operationClaimBusinessRules,
        IEfRepository efRepository)
    {
        _operationClaimBusinessRules = operationClaimBusinessRules;
        _efRepository = efRepository;
    }

    public async Task<Result<GetOperationClaimDto, Error>> Handle(UpdateOperationClaimCommandRequest request,
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

        await _efRepository.SaveChangesAsync(cancellationToken);

        return operationClaim.ToDto();
    }
}