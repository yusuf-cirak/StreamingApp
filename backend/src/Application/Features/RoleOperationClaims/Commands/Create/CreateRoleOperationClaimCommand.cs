using System.Security.Claims;
using Application.Common.Mapping;
using Application.Common.Rules;
using Application.Features.RoleOperationClaims.Dtos;
using Application.Features.RoleOperationClaims.Rules;

namespace Application.Features.RoleOperationClaims.Commands.Create;

public readonly record struct CreateRoleOperationClaimCommandRequest
    : IRequest<HttpResult<GetRoleOperationClaimDto>>, ISecuredRequest
{
    public Guid RoleId { get; init; }
    public Guid OperationClaimId { get; init; }
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public CreateRoleOperationClaimCommandRequest()
    {
        AuthorizationFunctions = [CommonAuthorizationRules.UserMustBeAdmin];
    }

    public CreateRoleOperationClaimCommandRequest(Guid roleId, Guid operationClaimId) : this()
    {
        RoleId = roleId;
        OperationClaimId = operationClaimId;
    }
}

public sealed class
    CreateRoleOperationClaimCommandHandler : IRequestHandler<CreateRoleOperationClaimCommandRequest,
    HttpResult<GetRoleOperationClaimDto>>
{
    private readonly IEfRepository _efRepository;
    private readonly RoleOperationClaimBusinessRules _roleOperationClaimBusinessRules;

    public CreateRoleOperationClaimCommandHandler(RoleOperationClaimBusinessRules roleOperationClaimBusinessRules,
        IEfRepository efRepository)
    {
        _roleOperationClaimBusinessRules = roleOperationClaimBusinessRules;
        _efRepository = efRepository;
    }

    public async Task<HttpResult<GetRoleOperationClaimDto>> Handle(CreateRoleOperationClaimCommandRequest request,
        CancellationToken cancellationToken)
    {
        var duplicateResult =
            await _roleOperationClaimBusinessRules.RoleOperationClaimMustNotExistBeforeRegistered(request.RoleId,
                request.OperationClaimId);

        if (duplicateResult.IsFailure)
        {
            return duplicateResult.Error;
        }

        var newRoleOperationClaim = RoleOperationClaim.Create(request.RoleId, request.OperationClaimId);

        _efRepository.RoleOperationClaims.Add(newRoleOperationClaim);

        var result = await _efRepository.SaveChangesAsync(cancellationToken);

        return result > 0
            ? HttpResult<GetRoleOperationClaimDto>.Success(newRoleOperationClaim.ToDto(), StatusCodes.Status201Created)
            : RoleOperationClaimErrors.CouldNotBeAdded;
    }
}