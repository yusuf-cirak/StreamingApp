namespace Application.Features.Users.Services;

public interface IUserService : IDomainService<User>
{
    Task<Result<User, Error>> UserMustExistAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<string> UploadProfileImageAsync(User user, IFormFile? file,
        string existingImageUrl);

    Task<List<GetStreamDto>> GetFollowingStreamsAsync(Guid userId,
        CancellationToken cancellationToken = default);

    IEnumerable<GetBlockedStreamDto> GetBlockedStreamsEnumerable(Guid currentUserId);
    Task UpdateUserProfileImageAsync(User user, string profileImageUrl);
}