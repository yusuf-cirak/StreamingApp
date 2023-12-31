using FluentValidation;

namespace Application.Features.StreamFollowerUsers.Commands.Delete;

public sealed class
    DeleteStreamFollowerUserCommandRequestValidator : AbstractValidator<DeleteStreamFollowerUserCommandRequest>
{
    public DeleteStreamFollowerUserCommandRequestValidator()
    {
        RuleFor(x => x.StreamerId)
            .NotEmpty()
            .WithMessage("StreamerId is required");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");
    }
}