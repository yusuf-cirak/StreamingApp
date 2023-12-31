using FluentValidation;

namespace Application.Features.Users.Commands.Update;

public sealed class UpdateUserCommandRequestValidator : AbstractValidator<UpdateUserCommandRequest>
{
    public UpdateUserCommandRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("UserId is required.");

        RuleFor(x => x.Username)
            .MaximumLength(50)
            .WithMessage("Username must not exceed {MaximumLength} characters.");
    }
}