using Application.Features.StreamBlockedUsers.Commands.Create;
using FluentValidation;

namespace Application.Features.StreamBlockedUsers.Commands.Delete;

public sealed class
    StreamBlockedUserDeleteCommandRequestValidator : AbstractValidator<StreamBlockedUserCreateCommandRequest>
{
    public StreamBlockedUserDeleteCommandRequestValidator()
    {
        RuleFor(r => r.StreamerId).NotEmpty().NotNull().WithMessage("StreamerId is required");
        RuleFor(r => r.BlockedUserId).NotEmpty().NotNull().WithMessage("BlockedUserId is required");
    }
}