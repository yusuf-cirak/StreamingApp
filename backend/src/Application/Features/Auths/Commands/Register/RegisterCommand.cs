using Application.Abstractions.Helpers;
using Application.Abstractions.Repository;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Features.Auths.Dtos;
using Application.Features.Auths.Rules;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auths.Commands.Register;

public readonly record struct RegisterCommandRequest(string Username, string Password) : IRequest<TokenResponseDto>;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterCommandRequest, TokenResponseDto>
{
    private readonly IEfRepository _efRepository;
    private readonly AuthBusinessRules _authBusinessRules;
    private readonly IJwtHelper _jwtHelper;
    private readonly IHashingHelper _hashingHelper;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public RegisterUserCommandHandler(AuthBusinessRules authBusinessRules, IJwtHelper jwtHelper, IHashingHelper hashingHelper, IHttpContextAccessor httpContextAccessor, IEfRepository efRepository)
    {
        _authBusinessRules = authBusinessRules;
        _jwtHelper = jwtHelper;
        _hashingHelper = hashingHelper;
        _httpContextAccessor = httpContextAccessor;
        _efRepository = efRepository;
    }

    public async Task<TokenResponseDto> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
    {
        await _authBusinessRules.UserNameCannotBeDuplicatedBeforeRegistered(request.Username);

        _hashingHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        User newUser = User.Create(request.Username, passwordHash, passwordSalt);

        var userIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

        AccessToken accessToken = _jwtHelper.CreateAccessToken(newUser);
        RefreshToken refreshToken = _jwtHelper.CreateRefreshToken(newUser, userIpAddress);


        _efRepository.Users.Add(newUser);
        _efRepository.RefreshTokens.Add(refreshToken);

        var saveResult = await _efRepository.SaveChangesAsync(cancellationToken);

        if (saveResult == 0)
        {
            throw new DatabaseOperationFailedException("Could not add user to database");
        }




        return new TokenResponseDto(accessToken.Token, refreshToken.Token);

    }
}