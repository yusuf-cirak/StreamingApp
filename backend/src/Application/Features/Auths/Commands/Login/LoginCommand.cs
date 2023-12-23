using Application.Abstractions.Helpers;
using Application.Abstractions.Repository;
using Application.Common.Models;
using Application.Features.Auths.Dtos;
using Application.Features.Auths.Rules;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auths.Commands.Login;

public readonly record struct LoginCommandRequest(string Username, string Password)
    : IRequest<Result<TokenResponseDto, Error>>;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommandRequest, Result<TokenResponseDto, Error>>
{
    private readonly AuthBusinessRules _authBusinessRules;
    private readonly IJwtHelper _jwtHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEfRepository _efRepository;

    public LoginCommandHandler(IJwtHelper jwtHelper, AuthBusinessRules authBusinessRules,
        IHttpContextAccessor httpContextAccessor, IEfRepository efRepository)
    {
        _jwtHelper = jwtHelper;
        _authBusinessRules = authBusinessRules;
        _httpContextAccessor = httpContextAccessor;
        _efRepository = efRepository;
    }

    public async Task<Result<TokenResponseDto, Error>> Handle(LoginCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authBusinessRules.UserNameShouldExistBeforeLogin(request.Username);

        if (result.IsFailure)
        {
            return result.Error;
        }

        User user = result.Value;

        _authBusinessRules.UserCredentialsMustMatchBeforeLogin(request.Password, user.PasswordHash, user.PasswordSalt);

        var userIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

        AccessToken accessToken = _jwtHelper.CreateAccessToken(user);
        RefreshToken refreshToken = _jwtHelper.CreateRefreshToken(user, userIpAddress);

        _efRepository.RefreshTokens.Add(refreshToken);

        await _efRepository.SaveChangesAsync(cancellationToken);

        return new TokenResponseDto(accessToken.Token, refreshToken.Token);
    }
}