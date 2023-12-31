using FluentValidation;

namespace Application.Features.StreamFollowerUsers.Commands.Create;

public class StreamFollowerUserCreateCommandRequestValidator : AbstractValidator<StreamFollowerUserCreateCommandRequest>
{
    public StreamFollowerUserCreateCommandRequestValidator()
    {
        RuleFor(x => x.StreamerId)
            .NotEmpty()
            .WithMessage("StreamerId is required");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");
    }
}