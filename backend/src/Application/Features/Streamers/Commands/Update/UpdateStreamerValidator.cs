using FluentValidation;

namespace Application.Features.Streamers.Commands.Update;

public sealed class UpdateStreamerCommandValidator : AbstractValidator<UpdateStreamerCommandRequest>
{
    public UpdateStreamerCommandValidator()
    {
        RuleFor(u => u.StreamTitle).NotEmpty().NotNull().MinimumLength(1)
            .WithMessage("{PropertyName} should at least be {MinLength} characters long");
        RuleFor(u => u.StreamDescription).NotEmpty().NotNull().MinimumLength(6)
            .WithMessage("{PropertyName} should at least be {MinLength} characters long");
    }
}