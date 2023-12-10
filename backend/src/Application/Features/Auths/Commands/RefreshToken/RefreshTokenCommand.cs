using Application.Abstractions.Helpers;
using Application.Abstractions.Repository;
using Application.Common.Exceptions;
using Application.Features.Auths.Dtos;
using Application.Features.Auths.Rules;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auths.Commands.Refresh;

public readonly record struct RefreshTokenCommandRequest(Guid UserId, string RefreshToken)
    : IRequest<TokenResponseDto>;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommandRequest, TokenResponseDto>
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

    public async Task<TokenResponseDto> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = (request.UserId);

        var user = await _authBusinessRules.UserWithIdMustExistBeforeRefreshToken(userId);

        await _authBusinessRules.GetAndVerifyUserRefreshToken(userId, request.RefreshToken);

        var userIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

        var accessToken = _jwtHelper.CreateAccessToken(user);
        var refreshToken = _jwtHelper.CreateRefreshToken(user, userIpAddress);

        _efRepository.RefreshTokens.Add(refreshToken);

        var saveResult = await _efRepository.SaveChangesAsync(cancellationToken);
        if (saveResult == 0)
        {
            throw new DatabaseOperationFailedException("Failed to add refreshed token to database");
        }

        return new(accessToken.Token, refreshToken.Token);
    }
}