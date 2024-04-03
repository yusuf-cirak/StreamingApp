using Application.Common.Mapping;
using Application.Common.Rules;
using Application.Features.OperationClaims.Rules;

namespace Application.Features.OperationClaims.Commands.Create;

public readonly record struct CreateOperationClaimCommandRequest : IRequest<HttpResult<GetOperationClaimDto>>,
    ISecuredRequest
{
    public string Name { get; init; }
    
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public CreateOperationClaimCommandRequest()
    {
        AuthorizationFunctions = [CommonAuthorizationRules.UserMustBeAdmin];
    }

    public CreateOperationClaimCommandRequest(string name) : this()
    {
        Name = name;
    }

}

public sealed class
    CreateOperationClaimCommandHandler : IRequestHandler<CreateOperationClaimCommandRequest,
    HttpResult<GetOperationClaimDto>>
{
    private readonly IEfRepository _efRepository;
    private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

    public CreateOperationClaimCommandHandler(OperationClaimBusinessRules operationClaimBusinessRules,
        IEfRepository efRepository)
    {
        _operationClaimBusinessRules = operationClaimBusinessRules;
        _efRepository = efRepository;
    }

    public async Task<HttpResult<GetOperationClaimDto>> Handle(CreateOperationClaimCommandRequest request,
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

        return HttpResult<GetOperationClaimDto>.Success(operationClaimDto, StatusCodes.Status201Created);
    }
}