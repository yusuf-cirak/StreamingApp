using Application.Abstractions.Security;
using Application.Common.Mapping;
using Application.Features.OperationClaims.Dtos;
using Application.Features.OperationClaims.Rules;

namespace Application.Features.OperationClaims.Commands.Create;

[AuthorizationPipeline(Roles = ["Admin"])]
public readonly record struct CreateOperationClaimCommandRequest(string Name)
    : IRequest<HttpResult<GetOperationClaimDto, Error>>, ISecuredRequest;

public sealed class
    CreateOperationClaimCommandHandler : IRequestHandler<CreateOperationClaimCommandRequest,
    HttpResult<GetOperationClaimDto, Error>>
{
    private readonly IEfRepository _efRepository;
    private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

    public CreateOperationClaimCommandHandler(OperationClaimBusinessRules operationClaimBusinessRules,
        IEfRepository efRepository)
    {
        _operationClaimBusinessRules = operationClaimBusinessRules;
        _efRepository = efRepository;
    }

    public async Task<HttpResult<GetOperationClaimDto, Error>> Handle(CreateOperationClaimCommandRequest request,
        CancellationToken cancellationToken)
    {
        var duplicateResult = await _operationClaimBusinessRules.OperationClaimNameCannotBeDuplicatedBeforeRegistered(
            request.Name);

        if (duplicateResult.IsFailure)
        {
            return duplicateResult.Error;
        }

        var operationClaim = OperationClaim.Create(request.Name);

        _efRepository.OperationClaims.Add(operationClaim);

        await _efRepository.SaveChangesAsync(cancellationToken);

        var operationClaimDto = operationClaim.ToDto();

        return HttpResult<GetOperationClaimDto, Error>.Success(operationClaimDto, StatusCodes.Status201Created);
    }
}