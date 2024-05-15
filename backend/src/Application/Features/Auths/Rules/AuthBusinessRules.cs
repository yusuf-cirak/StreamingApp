using Application.Abstractions;

namespace Application.Features.Auths.Rules;

public sealed class AuthBusinessRules : BaseBusinessRules
{
    // FromServices attribute could be used instead of constructor injection
    private readonly IEfRepository _repository;
    private readonly IHashingHelper _hashingHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthBusinessRules(IEfRepository repository, IHashingHelper hashingHelper,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _hashingHelper = hashingHelper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> UserNameCannotBeDuplicatedBeforeRegistered(string username)
    {
        User? user = await _repository.Users.SingleOrDefaultAsync(user => user.Username == username);

        if (user is not null)
        {
            return Result.Failure(UserErrors.NameCannotBeDuplicated);
        }

        return Result.Success();
    }


    public Result UserCredentialsMustMatchBeforeLogin(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        if (!_hashingHelper.VerifyPasswordHash(password, passwordHash, passwordSalt))
        {
            return Result.Failure(UserErrors.WrongCredentials);
        }

        return Result.Success();
    }

    public async Task<Result<User, Error>> UserNameShouldExistBeforeLogin(string username)
    {
        User? user = await _repository.Users.SingleOrDefaultAsync(user => user.Username == username);

        if (user is null)
        {
            return UserErrors.NotFound;
        }

        return user;
    }

    public async Task<Result<User, Error>> UserWithIdMustExistBeforeRefreshToken(Guid userId)
    {
        User? user = await _repository.Users.SingleOrDefaultAsync(user => user.Id == userId);


        if (user is null)
        {
            return UserErrors.NotFound;
        }

        return user;
    }

    public async Task<Result> GetAndVerifyUserRefreshToken(Guid userId, string refreshTokenFromRequest)
    {
        var ipAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "";

        var refreshToken = await _repository.RefreshTokens.OrderByDescending(rt => rt.ExpiresAt)
            .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.CreatedByIp == ipAddress);

        if (refreshToken is null)
        {
            return Result.Failure(RefreshTokenErrors.TokenNotFound);
        }

        if (refreshToken.Token != refreshTokenFromRequest)
        {
            return Result.Failure(RefreshTokenErrors.TokenIsNotValid);
        }

        if (refreshToken.ExpiresAt < DateTime.Now)
        {
            return Result.Failure(RefreshTokenErrors.TokenIsExpired);
        }

        return Result.Success();
    }
}