using Application.Features.Users.Services;

namespace Application.Features.Users.Commands.UpdateProfileImage;

public sealed record UpdateProfileImageCommandRequest : IRequest<HttpResult<string>>, ISecuredRequest
{
    public IFormFile? ProfileImage { get; set; }
    public string ProfileImageUrl { get; set; }
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public UpdateProfileImageCommandRequest()
    {
        AuthorizationFunctions = [];
    }
}

public sealed class
    UpdateProfileImageCommandHandler : IRequestHandler<UpdateProfileImageCommandRequest, HttpResult<string>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;

    public UpdateProfileImageCommandHandler(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HttpResult<string>> Handle(UpdateProfileImageCommandRequest request,
        CancellationToken cancellationToken)
    {
        Guid.TryParse(_httpContextAccessor.HttpContext!.User.GetUserId(), out Guid userId);

        var userResult = await _userService.UserMustExistAsync(userId, cancellationToken);

        if (userResult.IsFailure)
        {
            return userResult.Error;
        }

        var user = userResult.Value;


        var profileImageUrl = await _userService.UploadProfileImageAsync(user, request.ProfileImage,
            request.ProfileImageUrl);

        await _userService.UpdateUserProfileImageAsync(user, profileImageUrl);

        return profileImageUrl;
    }
}