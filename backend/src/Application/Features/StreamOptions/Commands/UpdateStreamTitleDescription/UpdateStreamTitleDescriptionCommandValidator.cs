using FluentValidation;

namespace Application.Features.StreamOptions.Commands.Update;

public sealed class UpdateStreamTitleDescriptionCommandValidator : AbstractValidator<UpdateStreamTitleDescriptionCommandRequest>
{
    public UpdateStreamTitleDescriptionCommandValidator()
    {
        RuleFor(u => u.StreamTitle).NotEmpty().NotNull().MinimumLength(1)
            .WithMessage("{PropertyName} should at least be {MinLength} characters long");
        RuleFor(u => u.StreamDescription).NotEmpty().NotNull().MinimumLength(6)
            .WithMessage("{PropertyName} should at least be {MinLength} characters long");
    }
}