using Application.Abstractions.Repository;
using Application.Abstractions.Security;
using Application.Common.Mapping;
using Application.Features.OperationClaims.Dtos;
using Application.Features.OperationClaims.Rules;

namespace Application.Features.OperationClaims.Commands.Create;

[AuthorizationPipeline(Roles = ["Admin"])]
public readonly record struct CreateOperationClaimCommandRequest(string Name)
    : IRequest<Result<GetOperationClaimDto, Error>>, ISecuredRequest;

public sealed class
    CreateOperationClaimCommandHandler : IRequestHandler<CreateOperationClaimCommandRequest,
    Result<GetOperationClaimDto, Error>>
{
    private readonly IEfRepository _efRepository;
    private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

    public CreateOperationClaimCommandHandler(OperationClaimBusinessRules operationClaimBusinessRules,
        IEfRepository efRepository)
    {
        _operationClaimBusinessRules = operationClaimBusinessRules;
        _efRepository = efRepository;
    }

    public async Task<Result<GetOperationClaimDto, Error>> Handle(CreateOperationClaimCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _operationClaimBusinessRules.OperationClaimNameCannotBeDuplicatedBeforeRegistered(
            request.Name);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var newOperationClaim = OperationClaim.Create(request.Name);

        _efRepository.OperationClaims.Add(newOperationClaim);

        await _efRepository.SaveChangesAsync(cancellationToken);

        return newOperationClaim.ToDto();
    }
}