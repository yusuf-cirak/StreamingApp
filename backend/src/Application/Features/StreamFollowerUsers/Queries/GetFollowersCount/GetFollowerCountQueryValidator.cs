using FluentValidation;

namespace Application.Features.StreamFollowerUsers.Queries.GetFollowersCount;

public sealed class GetFollowerCountQueryValidator : AbstractValidator<GetFollowersCountQueryRequest>
{
    public GetFollowerCountQueryValidator()
    {
        RuleFor(r => r.StreamerId).NotEmpty().NotNull().WithMessage("{PropertyName} cannot be empty");
    }
}