using Application.Common.Models;
using Application.Features.Auths.Rules;
using Application.Features.Auths.Services;

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

    public LoginCommandHandler(IJwtHelper jwtHelper, AuthBusinessRules authBusinessRules,
        IHttpContextAccessor httpContextAccessor, IEfRepository efRepository, IAuthService authService)
    {
        _jwtHelper = jwtHelper;
        _authBusinessRules = authBusinessRules;
        _httpContextAccessor = httpContextAccessor;
        _efRepository = efRepository;
        _authService = authService;
    }

    public async Task<HttpResult<AuthResponseDto>> Handle(LoginCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authBusinessRules.UserNameShouldExistBeforeLogin(request.Username);

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

        var userIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();


        var userRolesAndOperationClaims =
            await _authService.GetUserRolesAndOperationClaimsAsync(user.Id, cancellationToken);

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
            refreshToken.Token, accessToken.Expiration, claims: claimsDictionary);

        return authResponseDto;
    }
}