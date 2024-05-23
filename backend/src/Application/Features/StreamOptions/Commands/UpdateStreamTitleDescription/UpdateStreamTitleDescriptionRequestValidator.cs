using FluentValidation;

namespace Application.Features.StreamOptions.Commands.Update;

public sealed class
    UpdateStreamTitleDescriptionRequestValidator : AbstractValidator<UpdateStreamTitleDescriptionCommandRequest>
{
    public UpdateStreamTitleDescriptionRequestValidator()
    {
        RuleFor(r => r.StreamTitle).NotEmpty().NotNull().MinimumLength(4)
            .WithMessage("{PropertyName} should at least have {MinimumLength} characters");

        RuleFor(r => r.StreamDescription).NotEmpty().NotNull().MinimumLength(4)
            .WithMessage("{PropertyName} should at least have {MinimumLength} characters");


        RuleFor(r => r.Thumbnail).Must(NullOrNotBiggerThanMegabyte)
            .WithMessage("{PropertyName} cannot be bigger than 1 MB");
    }


    private bool NullOrNotBiggerThanMegabyte(object thumbnail)
    {
        // thumbnail is null || thumbnail.Length <= 1_048_576;

        if (thumbnail is IFormFile image)
        {
            return image.Length <= 1_048_576;
        }

        return true;
    }
}