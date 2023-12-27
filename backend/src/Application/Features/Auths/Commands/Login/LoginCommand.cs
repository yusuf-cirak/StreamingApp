using Application.Common.Models;
using Application.Features.Auths.Dtos;
using Application.Features.Auths.Rules;
using Application.Services;

namespace Application.Features.Auths.Commands.Login;

public readonly record struct LoginCommandRequest(string Username, string Password)
    : IRequest<HttpResult<TokenResponseDto>>;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommandRequest, HttpResult<TokenResponseDto>>
{
    private readonly AuthManager _authManager;
    private readonly AuthBusinessRules _authBusinessRules;
    private readonly IJwtHelper _jwtHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEfRepository _efRepository;

    public LoginCommandHandler(IJwtHelper jwtHelper, AuthBusinessRules authBusinessRules,
        IHttpContextAccessor httpContextAccessor, IEfRepository efRepository, AuthManager authManager)
    {
        _jwtHelper = jwtHelper;
        _authBusinessRules = authBusinessRules;
        _httpContextAccessor = httpContextAccessor;
        _efRepository = efRepository;
        _authManager = authManager;
    }

    public async Task<HttpResult<TokenResponseDto>> Handle(LoginCommandRequest request,
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
            await _authManager.GetUserRolesAndOperationClaimsAsync(user.Id, cancellationToken);

        var claimsDictionary = new Dictionary<string, dynamic>
        {
            { "Roles", userRolesAndOperationClaims.Roles },
            { "OperationClaims", userRolesAndOperationClaims.OperationClaims }
        };

        AccessToken accessToken = _jwtHelper.CreateAccessToken(user, claimsDictionary);
        RefreshToken refreshToken = _jwtHelper.CreateRefreshToken(user, userIpAddress);

        _efRepository.RefreshTokens.Add(refreshToken);

        await _efRepository.SaveChangesAsync(cancellationToken);

        return new TokenResponseDto(accessToken.Token, refreshToken.Token);
    }
}