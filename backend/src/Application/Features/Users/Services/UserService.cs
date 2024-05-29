using Application.Abstractions.Image;
using Application.Common.Mapping;
using Application.Contracts.Constants;
using Application.Features.Streams.Services;

namespace Application.Features.Users.Services;

public sealed class UserService : IUserService
{
    private readonly IEfRepository _efRepository;
    private readonly IStreamCacheService _cacheService;
    private readonly IImageService _imageService;

    public UserService(IEfRepository efRepository, IStreamCacheService cacheService, IImageService imageService)
    {
        _efRepository = efRepository;
        _cacheService = cacheService;
        _imageService = imageService;
    }


    public async Task<Result<User, Error>> UserMustExistAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _efRepository
            .Users
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);

        if (user is null)
        {
            return UserErrors.UserDoesNotExist;
        }

        return user;
    }

    public async Task<string> UploadProfileImageAsync(User user, IFormFile file, string existingImageUrl)
    {
        var profileImageUrl = string.Empty;
        if (file is not null && existingImageUrl.Length is 0)
        {
            profileImageUrl = await _imageService.UploadImageAsync(user.Id.ToString(), file,
                ImageConstants.Folder.ProfileImageFolder);
        }
        else if (existingImageUrl.Length > 0)
        {
            profileImageUrl = user.ProfileImageUrl;
        }
        else if (user.ProfileImageUrl.Length > 0)
        {
            _ = Task.Run(() => _imageService.DeleteImageAsync(user.ProfileImageUrl,
                ImageConstants.Folder.ProfileImageFolder));
        }


        return profileImageUrl;
    }

    public Task<List<GetStreamDto>> GetFollowingStreamsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return _efRepository.StreamFollowerUsers
            .Include(s => s.Streamer)
            .ThenInclude(s => s.StreamOption)
            .Where(sfu => sfu.UserId == userId)
            .Select(sfu => sfu.Streamer.ResolveGetStreamDto(_cacheService.LiveStreamers))
            .ToListAsync(cancellationToken);
    }

    public IEnumerable<GetBlockedStreamDto> GetBlockedStreamsEnumerable(Guid currentUserId)
    {
        return _efRepository.StreamBlockedUsers.Where(sbu => sbu.UserId == currentUserId)
            .Select(sbu => new GetBlockedStreamDto(sbu.Streamer.ToDto())).AsEnumerable();
    }

    public Task UpdateUserProfileImageAsync(User user, string profileImageUrl)
    {
        return _efRepository
            .Users
            .Where(u => u.Id == user.Id)
            .ExecuteUpdateAsync(usr => usr.SetProperty(prop => prop.ProfileImageUrl, profileImageUrl));
    }
}