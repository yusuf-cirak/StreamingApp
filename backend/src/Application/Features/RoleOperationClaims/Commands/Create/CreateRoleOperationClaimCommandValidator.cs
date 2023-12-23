using FluentValidation;

namespace Application.Features.RoleOperationClaims.Commands.Create;

public sealed class CreateRoleOperationClaimCommandValidator : AbstractValidator<CreateRoleOperationClaimCommandRequest>
{
    public CreateRoleOperationClaimCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("RoleId is required.");

        RuleFor(x => x.OperationClaimId)
            .NotEmpty()
            .WithMessage("OperationClaimId is required.");
    }
}