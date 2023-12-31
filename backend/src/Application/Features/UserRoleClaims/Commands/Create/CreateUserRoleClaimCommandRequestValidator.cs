using FluentValidation;

namespace Application.Features.UserRoleClaims.Commands.Create;

public sealed class CreateUserRoleClaimCommandRequestValidator : AbstractValidator<CreateUserRoleClaimCommandRequest>
{
    public CreateUserRoleClaimCommandRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("UserId is required");

        RuleFor(x => x.RoleId)
            .NotEmpty()
            .NotNull()
            .WithMessage("RoleId is required");

        RuleFor(x => x.Value)
            .NotEmpty()
            .NotNull()
            .WithMessage("Value is required");
    }
}