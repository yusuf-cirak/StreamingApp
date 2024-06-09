using Application.Contracts.Common.Models;
using Application.Features.Auths.Rules;
using Application.Features.Auths.Services;
using Application.Features.Users.Services;

namespace Application.Features.Auths.Commands.Login;

public readonly record struct LoginCommandRequest(string Username, string Password)
    : IRequest<HttpResult<AuthResponseDto>>;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommandRequest, HttpResult<AuthResponseDto>>
{
    private readonly IAuthService _authService;
    private readonly AuthBusinessRules _authBusinessRules;
    private readonly IJwtHelper _jwtHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEfRepository _efRepository;
    private readonly IUserBlacklistManager _blacklistManager;

    public LoginCommandHandler(IJwtHelper jwtHelper, AuthBusinessRules authBusinessRules,
        IHttpContextAccessor httpContextAccessor, IEfRepository efRepository, IAuthService authService,
        IUserBlacklistManager blacklistManager)
    {
        _jwtHelper = jwtHelper;
        _authBusinessRules = authBusinessRules;
        _httpContextAccessor = httpContextAccessor;
        _efRepository = efRepository;
        _authService = authService;
        _blacklistManager = blacklistManager;
    }

    public async Task<HttpResult<AuthResponseDto>> Handle(LoginCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authBusinessRules.UserNameShouldExistBeforeLoginAsync(request.Username, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        User user = result.Value;

        var verifyCredentialsResult =
            _authBusinessRules.UserCredentialsMustMatchBeforeLogin(request.Password, user.PasswordHash,
                user.PasswordSalt);

        if (verifyCredentialsResult.IsFailure)
        {
            return verifyCredentialsResult.Error;
        }

        var userIpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;


        var userRolesAndOperationClaims = _authService.GetUserRolesAndOperationClaims(user);

        var claimsDictionary = new Dictionary<string, dynamic>
        {
            { "roles", userRolesAndOperationClaims.Roles },
            { "operationClaims", userRolesAndOperationClaims.OperationClaims }
        };

        AccessToken accessToken = _jwtHelper.CreateAccessToken(user, claimsDictionary);
        RefreshToken refreshToken = _jwtHelper.CreateRefreshToken(user, userIpAddress);

        _efRepository.RefreshTokens.Add(refreshToken);

        await _efRepository.SaveChangesAsync(cancellationToken);

        var authResponseDto = new AuthResponseDto(user.Id, user.Username, user.ProfileImageUrl, accessToken.Token,
            refreshToken.Token, accessToken.Expiration, Claims: claimsDictionary);

        _ = Task.Run(() => _blacklistManager.RemoveUserFromBlacklistAsync(user.Id.ToString()));

        return authResponseDto;
    }
}