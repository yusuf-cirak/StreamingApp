using FluentValidation;

namespace Application.Features.Streams.Queries.Get;

public sealed class GetStreamQueryValidator : AbstractValidator<GetStreamQueryRequest>
{
    public GetStreamQueryValidator()
    {
        RuleFor(s => s.StreamerName).NotEmpty().NotNull().WithMessage("Streamer name must be provided");
    }
}