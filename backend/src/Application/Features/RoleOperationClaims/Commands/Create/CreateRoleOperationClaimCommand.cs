using Application.Abstractions.Security;
using Application.Features.RoleOperationClaims.Rules;

namespace Application.Features.RoleOperationClaims.Commands.Create;

public readonly record struct CreateRoleOperationClaimCommandRequest(Guid RoleId, Guid OperationClaimId)
    : IRequest<Result<bool, Error>>, ISecuredRequest;
    
    
    public sealed class CreateRoleOperationClaimCommandHandler : IRequestHandler<CreateRoleOperationClaimCommandRequest, Result<bool, Error>>
    {
        private readonly IEfRepository _efRepository;
        private readonly RoleOperationClaimBusinessRules _roleOperationClaimBusinessRules;

        public CreateRoleOperationClaimCommandHandler(RoleOperationClaimBusinessRules roleOperationClaimBusinessRules, IEfRepository efRepository)
        {
            _roleOperationClaimBusinessRules = roleOperationClaimBusinessRules;
            _efRepository = efRepository;
        }

        public async Task<Result<bool, Error>> Handle(CreateRoleOperationClaimCommandRequest request, CancellationToken cancellationToken)
        {
            var duplicateResult = await _roleOperationClaimBusinessRules.RoleOperationClaimMustNotExistBeforeRegistered(request.RoleId, request.OperationClaimId);

            if (duplicateResult.IsFailure)
            {
                return duplicateResult.Error;
            }

            var newRoleOperationClaim = RoleOperationClaim.Create(request.RoleId, request.OperationClaimId);

            _efRepository.RoleOperationClaims.Add(newRoleOperationClaim);

            await _efRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }