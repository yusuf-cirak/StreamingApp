using Application.Abstractions.Helpers;
using Application.Abstractions.Repository;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Features.Auths.Dtos;
using Application.Features.Auths.Rules;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auths.Commands.Login;

public readonly record struct LoginCommandRequest(string Username, string Password) : IRequest<TokenResponseDto>;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommandRequest, TokenResponseDto>
{
    private readonly AuthBusinessRules _authBusinessRules;
    private readonly IJwtHelper _jwtHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEfRepository _efRepository;

    public LoginCommandHandler(IJwtHelper jwtHelper, AuthBusinessRules authBusinessRules, IHttpContextAccessor httpContextAccessor, IEfRepository efRepository)
    {
        _jwtHelper = jwtHelper;
        _authBusinessRules = authBusinessRules;
        _httpContextAccessor = httpContextAccessor;
        _efRepository = efRepository;
    }

    public async Task<TokenResponseDto> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
    {
        User user = await _authBusinessRules.UserNameShouldExistBeforeLogin(request.Username);

        _authBusinessRules.UserCredentialsMustMatchBeforeLogin(request.Password, user.PasswordHash, user.PasswordSalt);

        var userIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

        AccessToken accessToken = _jwtHelper.CreateAccessToken(user);
        RefreshToken refreshToken = _jwtHelper.CreateRefreshToken(user,userIpAddress);

        _efRepository.RefreshTokens.Add(refreshToken);
        
        var saveResult = await _efRepository.SaveChangesAsync(cancellationToken);
        
        if (saveResult == 0)
        {
            throw new DatabaseOperationFailedException("Could not add refresh token to database");
        }

        return new TokenResponseDto(accessToken.Token, refreshToken.Token);
    }
}