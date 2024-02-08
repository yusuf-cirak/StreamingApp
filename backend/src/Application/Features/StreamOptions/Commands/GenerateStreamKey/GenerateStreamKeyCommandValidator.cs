using FluentValidation;

namespace Application.Features.StreamOptions.Commands.Update;

public sealed class GenerateStreamKeyCommandValidator : AbstractValidator<UpdateStreamChatSettingsCommandRequest>
{
    public GenerateStreamKeyCommandValidator()
    {
        RuleFor(u => u.StreamerId).NotEmpty().NotNull()
            .WithMessage("{PropertyName} should cannot be empty");
    }
}