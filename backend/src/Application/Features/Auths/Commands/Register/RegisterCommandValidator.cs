using FluentValidation;

namespace Application.Features.Auths.Commands.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommandRequest>
{
    public RegisterCommandValidator()
    {
        RuleFor(u => u.Username).NotEmpty().WithMessage("User name cannot be empty");
        RuleFor(u => u.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be longer than 6 characters");
    }
}