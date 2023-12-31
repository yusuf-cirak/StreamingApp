using FluentValidation;

namespace Application.Features.UserOperationClaims.Commands.Create;

public sealed class
    CreateUserOperationClaimCommandRequestValidator : AbstractValidator<CreateUserOperationClaimCommandRequest>
{
    public CreateUserOperationClaimCommandRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("UserId is required");

        RuleFor(x => x.OperationClaimId)
            .NotEmpty()
            .NotNull()
            .WithMessage("OperationClaimId is required");

        RuleFor(x => x.Value)
            .NotEmpty()
            .NotNull()
            .WithMessage("Value is required");
    }
}