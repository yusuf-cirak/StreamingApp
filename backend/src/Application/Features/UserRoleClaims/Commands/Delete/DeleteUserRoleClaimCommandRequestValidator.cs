using FluentValidation;

namespace Application.Features.UserRoleClaims.Commands.Delete;

public sealed class DeleteUserRoleClaimCommandRequestValidator : AbstractValidator<DeleteUserRoleClaimCommandRequest>
{
    public DeleteUserRoleClaimCommandRequestValidator()
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