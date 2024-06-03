using Application.Common.Mapping;
using Application.Common.Permissions;
using Application.Features.RoleOperationClaims.Rules;

namespace Application.Features.RoleOperationClaims.Commands.Create;

public readonly record struct CreateRoleOperationClaimCommandRequest()
    : IRequest<HttpResult<GetRoleOperationClaimDto>>, ISecuredRequest
{
    public Guid RoleId { get; init; }
    public Guid OperationClaimId { get; init; }
    public PermissionRequirements PermissionRequirements { get; } = PermissionRequirementConstants.WithAdminRole();
}

public sealed class
    CreateRoleOperationClaimCommandHandler(
        RoleOperationClaimBusinessRules roleOperationClaimBusinessRules,
        IEfRepository efRepository)
    : IRequestHandler<CreateRoleOperationClaimCommandRequest,
        HttpResult<GetRoleOperationClaimDto>>
{
    public async Task<HttpResult<GetRoleOperationClaimDto>> Handle(CreateRoleOperationClaimCommandRequest request,
        CancellationToken cancellationToken)
    {
        var duplicateResult =
            await roleOperationClaimBusinessRules.RoleOperationClaimMustNotExistBeforeRegistered(request.RoleId,
                request.OperationClaimId);

        if (duplicateResult.IsFailure)
        {
            return duplicateResult.Error;
        }

        var newRoleOperationClaim = RoleOperationClaim.Create(request.RoleId, request.OperationClaimId);

        efRepository.RoleOperationClaims.Add(newRoleOperationClaim);

        var result = await efRepository.SaveChangesAsync(cancellationToken);

        return result > 0
            ? HttpResult<GetRoleOperationClaimDto>.Success(newRoleOperationClaim.ToDto(), StatusCodes.Status201Created)
            : RoleOperationClaimErrors.CouldNotBeAdded;
    }
}