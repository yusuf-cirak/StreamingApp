using FluentValidation;

namespace Application.Features.Roles.Commands.Delete;

public sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommandRequest>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
    }
}