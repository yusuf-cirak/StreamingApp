﻿using Application.Common.Models;
using Application.Features.Auths.Dtos;
using Application.Features.Auths.Rules;

namespace Application.Features.Auths.Commands.Register;

public readonly record struct RegisterCommandRequest(string Username, string Password)
    : IRequest<HttpResult<TokenResponseDto>>;

public sealed class
    RegisterUserCommandHandler : IRequestHandler<RegisterCommandRequest, HttpResult<TokenResponseDto>>
{
    private readonly IEfRepository _efRepository;
    private readonly AuthBusinessRules _authBusinessRules;
    private readonly IJwtHelper _jwtHelper;
    private readonly IHashingHelper _hashingHelper;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IEncryptionHelper _encryptionHelper;

    public RegisterUserCommandHandler(AuthBusinessRules authBusinessRules, IJwtHelper jwtHelper,
        IHashingHelper hashingHelper, IHttpContextAccessor httpContextAccessor, IEfRepository efRepository,
        IEncryptionHelper encryptionHelper)
    {
        _authBusinessRules = authBusinessRules;
        _jwtHelper = jwtHelper;
        _hashingHelper = hashingHelper;
        _httpContextAccessor = httpContextAccessor;
        _efRepository = efRepository;
        _encryptionHelper = encryptionHelper;
    }

    public async Task<HttpResult<TokenResponseDto>> Handle(RegisterCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authBusinessRules.UserNameCannotBeDuplicatedBeforeRegistered(request.Username);

        if (result.IsFailure)
        {
            return result.Error;
        }

        _hashingHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        // User created, User create event should be fired and create a streamer for the user
        User newUser = User.Create(request.Username, passwordHash, passwordSalt);

        var userIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

        AccessToken accessToken = _jwtHelper.CreateAccessToken(newUser, new());
        RefreshToken refreshToken = _jwtHelper.CreateRefreshToken(newUser, userIpAddress);

        _efRepository.Users.Add(newUser);
        _efRepository.RefreshTokens.Add(refreshToken);

        // All users are also streamers, so we create a streamer for the user
        // TODO: Handle this with events
        var streamerKey = _encryptionHelper.Encrypt(newUser.Id.ToString());
        var streamerTitle = $"{newUser.Username}'s stream";
        var streamerDescription = $"{newUser.Username}'s stream description";

        var streamer = Streamer.Create(newUser.Id, streamerKey, streamerTitle, streamerDescription);

        _efRepository.Streamers.Add(streamer);

        await _efRepository.SaveChangesAsync(cancellationToken);

        var tokenResponseDto = new TokenResponseDto(accessToken.Token, refreshToken.Token);

        return HttpResult<TokenResponseDto>.Success(tokenResponseDto,
            StatusCodes.Status201Created);
    }
}