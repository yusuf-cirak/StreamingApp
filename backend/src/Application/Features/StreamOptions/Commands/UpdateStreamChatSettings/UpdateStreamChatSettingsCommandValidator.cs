using FluentValidation;

namespace Application.Features.StreamOptions.Commands.Update;

public sealed class UpdateStreamChatSettingsCommandValidator : AbstractValidator<UpdateStreamChatSettingsCommandRequest>
{
    public UpdateStreamChatSettingsCommandValidator()
    {
        RuleFor(u => u.ChatDisabled).NotNull()
            .WithMessage("{PropertyName} cannot be empty");
        RuleFor(u => u.ChatDelaySecond).NotNull().LessThanOrEqualTo(60)
            .WithMessage("{PropertyName} cannot be longer than 60 seconds");
        RuleFor(u => u.MustBeFollower).NotNull()
            .WithMessage("{PropertyName} cannot be empty");
    }
}