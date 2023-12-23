using FluentValidation;

namespace Application.Features.OperationClaims.Commands.Create;

public sealed class CreateOperationClaimCommandValidator : AbstractValidator<CreateOperationClaimCommandRequest>
{
    public CreateOperationClaimCommandValidator()
    {
        RuleFor(u => u.Name).NotEmpty().NotNull().MinimumLength(3)
            .WithMessage("{PropertyName} should at least be {MinLength} characters long");
    }
}