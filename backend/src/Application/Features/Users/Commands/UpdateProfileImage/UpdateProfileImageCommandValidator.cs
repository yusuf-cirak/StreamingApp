using FluentValidation;

namespace Application.Features.Users.Commands.UpdateProfileImage;

public sealed class UpdateProfileImageCommandValidator : AbstractValidator<UpdateProfileImageCommandRequest>
{
    public UpdateProfileImageCommandValidator()
    {
        RuleFor(x => x.ProfileImage).Must(NullOrNotBiggerThanMegabyte);
    }


    private bool NullOrNotBiggerThanMegabyte(object thumbnail)
    {
        if (thumbnail is IFormFile image)
        {
            return image.Length <= 524_288;
        }

        return true;
    }
}