using FluentValidation;

namespace Application.Features.Auths.Commands.Refresh;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommandRequest>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(u => u.UserId).NotEmpty().NotNull().WithMessage("UserId cannot be empty");
        RuleFor(u => u.RefreshToken).NotEmpty().NotNull().WithMessage("Refresh token cannot be empty");
    }
}