using FluentValidation;

namespace Application.Features.StreamOptions.Commands.Update;

public sealed class UpdateStreamChatSettingsCommandValidator : AbstractValidator<UpdateStreamChatSettingsCommandRequest>
{
    public UpdateStreamChatSettingsCommandValidator()
    {
        RuleFor(u => u.ChatDisabled).NotEmpty()
            .WithMessage("{PropertyName} should cannot be empty");
        RuleFor(u => u.ChatDelaySecond).NotEmpty().LessThanOrEqualTo(60)
            .WithMessage("{PropertyName} cannot be longer than 60 seconds");
        RuleFor(u => u.MustBeFollower).NotEmpty()
            .WithMessage("{PropertyName} should cannot be empty");
    }
}