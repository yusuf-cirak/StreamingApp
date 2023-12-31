using FluentValidation;

namespace Application.Features.UserOperationClaims.Commands.Delete;

public sealed class
    DeleteUserOperationClaimCommandRequestValidator : AbstractValidator<DeleteUserOperationClaimCommandRequest>
{
    public DeleteUserOperationClaimCommandRequestValidator()
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