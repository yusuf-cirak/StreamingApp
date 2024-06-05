using FluentValidation;

namespace Application.Features.Users.Queries.GetByTerm;

public class GetUsersByTermQueryRequestValidator : AbstractValidator<GetUsersByTermQueryRequest>
{
    public GetUsersByTermQueryRequestValidator()
    {
        RuleFor(r => r.Term)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .WithMessage("{PropertyName} should at least be at length of {MinimumLength}");
    }
}