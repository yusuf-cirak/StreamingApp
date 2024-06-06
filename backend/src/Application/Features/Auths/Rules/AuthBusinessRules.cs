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

    public async Task<Result<User, Error>> UserNameShouldExistBeforeLoginAsync(string username,
        CancellationToken cancellationToken)
    {
        var user = await this
            .GetUserQueryable()
            .SingleOrDefaultAsync(u => u.Username == username, cancellationToken);

        if (user is null)
        {
            return UserErrors.NotFound;
        }

        return user;
    }

    public async Task<Result<User, Error>> UserWithIdMustExist(Guid userId, string ipAddress)
    {
        var result = await this
            .GetUserQueryable()
            .AsSplitQuery()
            .Where(u => u.Id == userId)
            .Select(user => new
            {
                User = user,
                RefreshToken = user.RefreshTokens
                    .OrderByDescending(rt => rt.ExpiresAt)
                    .FirstOrDefault(rt => rt.CreatedByIp == ipAddress)
            }).SingleOrDefaultAsync();

        if (result is null)
        {
            return UserErrors.NotFound;
        }

        if (result.RefreshToken is not null)
        {
            result.User.RefreshTokens.Add(result.RefreshToken);
        }


        return result.User;
    }

    public Result VerifyRefreshToken(User user, string refreshTokenFromRequest)
    {
        var refreshToken = user.RefreshTokens.FirstOrDefault();

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

    private IQueryable<User> GetUserQueryable() => _repository
        .Users
        .Include(u => u.UserRoleClaims)
        .ThenInclude(urc => urc.Role)
        .Include(u => u.UserOperationClaims)
        .ThenInclude(uoc => uoc.OperationClaim);
    // .AsSplitQuery();
}