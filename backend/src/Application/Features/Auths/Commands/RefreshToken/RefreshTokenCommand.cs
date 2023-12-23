using Application.Abstractions.Helpers;
using Application.Abstractions.Repository;
using Application.Common.Exceptions;
using Application.Features.Auths.Dtos;
using Application.Features.Auths.Rules;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auths.Commands.Refresh;

public readonly record struct RefreshTokenCommandRequest(Guid UserId, string RefreshToken)
    : IRequest<Result<TokenResponseDto, Error>>;

public sealed class
    RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommandRequest, Result<TokenResponseDto, Error>>
{
    private readonly IEfRepository _efRepository;
    private readonly IJwtHelper _jwtHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AuthBusinessRules _authBusinessRules;

    public RefreshTokenCommandHandler(IJwtHelper jwtHelper, IHttpContextAccessor httpContextAccessor,
        AuthBusinessRules authBusinessRules, IEfRepository efRepository)
    {
        _jwtHelper = jwtHelper;
        _httpContextAccessor = httpContextAccessor;
        _authBusinessRules = authBusinessRules;
        _efRepository = efRepository;
    }

    public async Task<Result<TokenResponseDto, Error>> Handle(RefreshTokenCommandRequest request,
        CancellationToken cancellationToken)
    {
        var userId = (request.UserId);

        var userExistResult = await _authBusinessRules.UserWithIdMustExistBeforeRefreshToken(userId);

        if (userExistResult.IsFailure)
        {
            return userExistResult.Error;
        }

        User user = userExistResult.Value;

        var verifyRefreshTokenResult =
            await _authBusinessRules.GetAndVerifyUserRefreshToken(userId, request.RefreshToken);

        if (verifyRefreshTokenResult.IsFailure)
        {
            return verifyRefreshTokenResult.Error;
        }

        var userIpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "";

        var accessToken = _jwtHelper.CreateAccessToken(user);
        var refreshToken = _jwtHelper.CreateRefreshToken(user, userIpAddress);

        _efRepository.RefreshTokens.Add(refreshToken);

        await _efRepository.SaveChangesAsync(cancellationToken);

        return new TokenResponseDto(accessToken.Token, refreshToken.Token);
    }
}