using Application.Abstractions;
using Application.Abstractions.Helpers;
using Application.Abstractions.Repository;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auths.Rules;

public sealed class AuthBusinessRules : BaseBusinessRules
{

    // FromServices attribute could be used instead of constructor injection
    private readonly IEfRepository _repository;
    private readonly IHashingHelper _hashingHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthBusinessRules(IEfRepository repository, IHashingHelper hashingHelper, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _hashingHelper = hashingHelper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task UserNameCannotBeDuplicatedBeforeRegistered(string username)
    {
        User? user = await _repository.Users.SingleOrDefaultAsync(user => user.Username == username);

        if (user is not null)
        {
            throw new BusinessException("There is already a user with that user name");
        }

    }


    public void UserCredentialsMustMatchBeforeLogin(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        if (!_hashingHelper.VerifyPasswordHash(password, passwordHash, passwordSalt))
        {
            throw new BusinessException("Wrong credentials");
        }
    }

    public async Task<User> UserNameShouldExistBeforeLogin(string username)
    {
        User? user = await _repository.Users.SingleOrDefaultAsync(user => user.Username == username);

        if (user is null)
        {
            throw new BusinessException("There is no user with that user name");
        }

        return user;
    }

    public async Task<User> UserWithIdMustExistBeforeRefreshToken(Guid userId)
    {
        User? user = await _repository.Users.SingleOrDefaultAsync(user => user.Id == userId);


        if (user is null)
        {
            throw new BusinessException("There is no user with that user id");
        }

        return user;
    }

    public async Task GetAndVerifyUserRefreshToken(Guid userId, string refreshTokenFromRequest)
    {
        var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

        var refreshToken = await _repository.RefreshTokens.LastOrDefaultAsync(rt => rt.UserId == userId && rt.CreatedByIp == ipAddress);


        if (refreshToken is null || refreshToken.Token != refreshTokenFromRequest || refreshToken.ExpiresAt < DateTime.Now)
        {
            throw new BusinessException("Refresh token is not valid");
        }
    }
}