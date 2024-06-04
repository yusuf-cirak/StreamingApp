using Application.Abstractions.Locking;
using Application.Features.Auths.Rules;
using Application.Features.Auths.Services;

namespace Application.Features.Auths.Commands.Refresh;

public readonly record struct RefreshTokenCommandRequest
    : ILockRequest, IRequest<HttpResult<AuthResponseDto>>
{
    public Guid UserId { get; init; }
    public string RefreshToken { get; init; }
    public string Key => UserId.ToString();
    public int Expiration => 30;
    public bool ReleaseImmediately => true;
}

public sealed class
    RefreshTokenCommandHandler(
        IJwtHelper jwtHelper,
        IHttpContextAccessor httpContextAccessor,
        AuthBusinessRules authBusinessRules,
        IEfRepository efRepository,
        IAuthService authService)
    : IRequestHandler<RefreshTokenCommandRequest, HttpResult<AuthResponseDto>>
{
    public async Task<HttpResult<AuthResponseDto>> Handle(RefreshTokenCommandRequest request,
        CancellationToken cancellationToken)
    {
        var userId = (request.UserId);

        var userExistResult = await authBusinessRules.UserWithIdMustExistBeforeRefreshToken(userId);

        if (userExistResult.IsFailure)
        {
            return userExistResult.Error;
        }

        User user = userExistResult.Value;

        var verifyRefreshTokenResult =
            await authBusinessRules.GetAndVerifyUserRefreshToken(userId, request.RefreshToken);

        if (verifyRefreshTokenResult.IsFailure)
        {
            return verifyRefreshTokenResult.Error;
        }

        var userIpAddress = httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "";

        var userRolesAndOperationClaims =
            await authService.GetUserRolesAndOperationClaimsAsync(user.Id, cancellationToken);

        var claimsDictionary = new Dictionary<string, object>
        {
            { "roles", userRolesAndOperationClaims.Roles },
            { "operationClaims", userRolesAndOperationClaims.OperationClaims }
        };

        var accessToken = jwtHelper.CreateAccessToken(user, claimsDictionary);
        var refreshToken = jwtHelper.CreateRefreshToken(user, userIpAddress);

        efRepository.RefreshTokens.Add(refreshToken);

        await efRepository.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(user.Id, user.Username, user.ProfileImageUrl, accessToken.Token, refreshToken.Token,
            accessToken.Expiration, claimsDictionary);
    }
}