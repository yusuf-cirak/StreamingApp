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

        var ipAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;

        var userExistResult = await authBusinessRules.UserWithIdMustExist(userId, ipAddress);

        if (userExistResult.IsFailure)
        {
            return userExistResult.Error;
        }

        var user = userExistResult.Value;

        var verifyRefreshTokenResult =
            authBusinessRules.VerifyRefreshToken(user, request.RefreshToken);

        if (verifyRefreshTokenResult.IsFailure)
        {
            return verifyRefreshTokenResult.Error;
        }

        var userRolesAndOperationClaims =
            authService.GetUserRolesAndOperationClaims(user);

        var claimsDictionary = new Dictionary<string, object>
        {
            { "roles", userRolesAndOperationClaims.Roles },
            { "operationClaims", userRolesAndOperationClaims.OperationClaims }
        };

        var accessToken = jwtHelper.CreateAccessToken(user, claimsDictionary);
        var refreshToken = jwtHelper.CreateRefreshToken(user, ipAddress);

        efRepository.RefreshTokens.Add(refreshToken);

        await efRepository.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(user.Id, user.Username, user.ProfileImageUrl, accessToken.Token, refreshToken.Token,
            accessToken.Expiration, claimsDictionary);
    }
}