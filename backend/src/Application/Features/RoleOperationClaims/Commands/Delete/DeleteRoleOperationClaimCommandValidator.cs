using FluentValidation;

namespace Application.Features.RoleOperationClaims.Commands.Delete;

public sealed class DeleteRoleOperationClaimCommandValidator : AbstractValidator<DeleteRoleOperationClaimCommandRequest>
{
    public DeleteRoleOperationClaimCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("RoleId is required.");

        RuleFor(x => x.OperationClaimId)
            .NotEmpty()
            .WithMessage("OperationClaimId is required.");
    }
}