using Application.Features.Auths.Rules;
using Application.Features.Auths.Services;
using Application.Features.StreamOptions.Services;

namespace Application.Features.Auths.Commands.Register;

public readonly record struct RegisterCommandRequest(string Username, string Password)
    : IRequest<HttpResult<AuthResponseDto>>;

public sealed class
    RegisterUserCommandHandler : IRequestHandler<RegisterCommandRequest, HttpResult<AuthResponseDto>>
{
    private readonly IEfRepository _efRepository;
    private readonly AuthBusinessRules _authBusinessRules;
    private readonly IAuthService _authService;
    private readonly IStreamOptionService _streamOptionService;

    public RegisterUserCommandHandler(AuthBusinessRules authBusinessRules, IEfRepository efRepository,
        IAuthService authService, IStreamOptionService streamOptionService)
    {
        _authBusinessRules = authBusinessRules;
        _efRepository = efRepository;
        _authService = authService;
        _streamOptionService = streamOptionService;
    }

    public async Task<HttpResult<AuthResponseDto>> Handle(RegisterCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authBusinessRules.UserNameCannotBeDuplicatedBeforeRegistered(request.Username);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var user = _authService.RegisterUser(request.Username, request.Password, out var accessToken,
            out var refreshToken, out var claims);

        _streamOptionService.CreateStreamOption(user);

        await _efRepository.SaveChangesAsync(cancellationToken);

        var authResponseDto = new AuthResponseDto(user.Id, user.Username, ProfileImageUrl: string.Empty,
            accessToken.Token, refreshToken.Token, accessToken.Expiration, Claims: claims);

        return HttpResult<AuthResponseDto>.Success(authResponseDto,
            StatusCodes.Status201Created);
    }
}