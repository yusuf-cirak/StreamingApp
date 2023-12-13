using FluentValidation;

namespace Application.Features.Streamers.Commands.Create;

public sealed class CreateStreamerCommandValidator : AbstractValidator<CreateStreamerCommandRequest>
{
    public CreateStreamerCommandValidator()
    {
        RuleFor(u => u.UserId).NotEmpty().WithMessage("User name cannot be empty");
    }
}