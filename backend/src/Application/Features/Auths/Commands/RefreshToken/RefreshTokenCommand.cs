using Application.Features.Auths.Dtos;
using Application.Features.Auths.Rules;
using Application.Features.Auths.Services;

namespace Application.Features.Auths.Commands.Refresh;

public readonly record struct RefreshTokenCommandRequest
    : IRequest<HttpResult<AuthResponseDto>>
{
    public Guid UserId { get; init; }
    public string RefreshToken { get; init; }
}

public sealed class
    RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommandRequest, HttpResult<AuthResponseDto>>
{
    private readonly IEfRepository _efRepository;
    private readonly IJwtHelper _jwtHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AuthBusinessRules _authBusinessRules;
    private readonly IAuthService _authService;

    public RefreshTokenCommandHandler(IJwtHelper jwtHelper, IHttpContextAccessor httpContextAccessor,
        AuthBusinessRules authBusinessRules, IEfRepository efRepository, IAuthService authService)
    {
        _jwtHelper = jwtHelper;
        _httpContextAccessor = httpContextAccessor;
        _authBusinessRules = authBusinessRules;
        _efRepository = efRepository;
        _authService = authService;
    }

    public async Task<HttpResult<AuthResponseDto>> Handle(RefreshTokenCommandRequest request,
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

        var userRolesAndOperationClaims =
            await _authService.GetUserRolesAndOperationClaimsAsync(user.Id, cancellationToken);

        var claimsDictionary = new Dictionary<string, object>
        {
            { "Roles", userRolesAndOperationClaims.Roles },
            { "OperationClaims", userRolesAndOperationClaims.OperationClaims }
        };

        var accessToken = _jwtHelper.CreateAccessToken(user, claimsDictionary);
        var refreshToken = _jwtHelper.CreateRefreshToken(user, userIpAddress);

        _efRepository.RefreshTokens.Add(refreshToken);

        await _efRepository.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(user.Id, user.Username, user.ProfileImageUrl, accessToken.Token, refreshToken.Token,
            accessToken.Expiration, claimsDictionary);
    }
}