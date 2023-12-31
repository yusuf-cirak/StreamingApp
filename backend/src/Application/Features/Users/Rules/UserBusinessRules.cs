using Application.Abstractions;

namespace Application.Features.Users.Rules;

public sealed class UserBusinessRules : BaseBusinessRules
{
    private readonly IEfRepository _efRepository;

    public UserBusinessRules(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<Result<User, Error>> UserMustExistBeforeUpdated(Guid userId)
    {
        var user = await _efRepository.Users.SingleOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            return UserErrors.NameCannotBeLongerThan50Characters;
        }

        return user;
    }

    public bool ShouldUpdateUsername(string username)
    {
        username = username.Trim();

        return username != string.Empty && username.Length < 50;
    }
    
    public bool ShouldUpdatePassword(string oldPassword, string newPassword)
    {
        if (oldPassword == string.Empty && newPassword == string.Empty)
        {
            return false;
        }

        if (oldPassword == newPassword)
        {
            return false;
        }

        if (newPassword.Length < 8)
        {
            return false;
        }

        if (newPassword.Length > 50)
        {
            return false;
        }

        return true;
    }
}