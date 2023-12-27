using FluentValidation;

namespace Application.Features.StreamBlockedUsers.Commands.Create;

public sealed class
    StreamBlockedUserCreateCommandRequestValidator : AbstractValidator<StreamBlockedUserCreateCommandRequest>
{
    public StreamBlockedUserCreateCommandRequestValidator()
    {
        RuleFor(r => r.BlockedUserId).NotEmpty().NotNull().WithMessage("BlockedUserId is required");

        RuleFor(r => r.StreamerId).NotEmpty().NotNull().WithMessage("StreamerId is required");
    }
}