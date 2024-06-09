using Application.Common.Permissions;
using Application.Features.Users.Abstractions;
using Application.Features.Users.Rules;

namespace Application.Features.Users.Commands.Update;

public record struct UpdateUserCommandRequest() : IUserCommandRequest, IRequest<HttpResult>, IPermissionRequest
{
    private Guid _userId;

    public Guid UserId
    {
        get => _userId;
        set
        {
            _userId = value;

            PermissionRequirements = PermissionRequirementConstants.WithNameIdentifier(value.ToString());
        }
    }

    public string Username { get; init; } = string.Empty;

    public string OldPassword { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
    public PermissionRequirements PermissionRequirements { get; private set; }
}

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly UserBusinessRules _userBusinessRules;
    private readonly IHashingHelper _hashingHelper;

    public UpdateUserCommandHandler(IEfRepository efRepository, UserBusinessRules userBusinessRules,
        IHashingHelper hashingHelper)
    {
        _efRepository = efRepository;
        _userBusinessRules = userBusinessRules;
        _hashingHelper = hashingHelper;
    }


    public async Task<HttpResult> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var userResult = await _userBusinessRules.UserMustExistBeforeUpdated(request.UserId);

        if (userResult.IsFailure)
        {
            return userResult.Error;
        }

        var user = userResult.Value;

        var shouldUpdateUserName = _userBusinessRules.ShouldUpdateUsername(request.Username);

        if (shouldUpdateUserName)
        {
            user.Username = request.Username;
        }

        var shouldUpdatePassword = _userBusinessRules.ShouldUpdatePassword(request.OldPassword, request.NewPassword);

        if (shouldUpdatePassword)
        {
            var verifyOldPassword = _hashingHelper.VerifyPasswordHash(request.OldPassword, user.PasswordHash,
                user.PasswordSalt);

            if (verifyOldPassword)
            {
                _hashingHelper.CreatePasswordHash(request.NewPassword, out var passwordHash, out var passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
        }


        _efRepository.Users.Update(user);

        var result = await _efRepository.SaveChangesAsync(cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : HttpResult.Failure(UserErrors.PasswordIsNotUpdated);
    }
}