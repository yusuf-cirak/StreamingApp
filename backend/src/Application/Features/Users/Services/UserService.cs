using Application.Abstractions.Image;
using Application.Common.Mapping;
using Application.Contracts.Constants;
using Application.Features.Streams.Services;

namespace Application.Features.Users.Services;

public interface IUserService : IDomainService<User>
{
    IEnumerable<GetUserDto> SearchUsersByName(string term);
    Task<Result<User, Error>> UserMustExistAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<string> UploadProfileImageAsync(User user, IFormFile? file,
        string existingImageUrl);

    Task<List<GetStreamDto>> GetFollowingStreamsAsync(Guid userId,
        CancellationToken cancellationToken = default);

    IEnumerable<GetBlockedStreamDto> GetBlockedStreamsEnumerable(Guid currentUserId);
    Task UpdateUserProfileImageAsync(User user, string profileImageUrl);

    Task<Result<List<User>, Error>> GetUsersByIdsAsync(List<Guid> userIds,
        CancellationToken cancellationToken = default);

    void UpdateRoleClaims(User user, List<Guid> roleIds, string value);
    void UpdateOperationClaims(User user, List<Guid> operationClaimIds, string value);

    void UpdateUsersWithRolesAndOperationClaims(List<User> users, List<Guid> roleIds, List<Guid> operationClaimIds,
        string value);

    void DeleteStreamModerators(List<User> users,
        string value);
}

public sealed class UserService : IUserService
{
    private readonly IEfRepository _efRepository;
    private readonly IStreamCacheService _streamCacheService;
    private readonly IImageService _imageService;

    public UserService(IEfRepository efRepository, IStreamCacheService streamCacheService, IImageService imageService)
    {
        _efRepository = efRepository;
        _streamCacheService = streamCacheService;
        _imageService = imageService;
    }

    public IEnumerable<GetUserDto> SearchUsersByName(string term)
    {
        var searchTerm = $"%{term.ToLower()}%";
        return _efRepository
            .Users
            .Where(u => EF.Functions.Like(u.Username.ToLower(), searchTerm))
            .Select(u => u.ToDto());
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
            .Select(sfu => sfu.Streamer.ResolveGetStreamDto(_streamCacheService.LiveStreamers))
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

    public async Task<Result<List<User>, Error>> GetUsersByIdsAsync(List<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        var users = await _efRepository
            .Users
            .AsTracking()
            .Where(user => userIds.Contains(user.Id))
            .Include(u => u.UserRoleClaims)
            .Include(u => u.UserOperationClaims)
            .ToListAsync(cancellationToken);


        if (users.Count != userIds.Count)
        {
            return Error.Create("UserError", "Users with ids are not found");
        }

        return users;
    }

    public void UpdateRoleClaims(User user, List<Guid> roleIds, string value)
    {
        var roleClaims = user.UserRoleClaims.Where(urc => urc.Value != value).ToList();
        roleIds.ForEach(roleId =>
        {
            var userRoleClaim = UserRoleClaim.Create(user.Id, roleId, value);
            roleClaims.Add(userRoleClaim);
        });

        user.UserRoleClaims = roleClaims;
        Console.WriteLine();
    }

    public void UpdateOperationClaims(User user, List<Guid> operationClaimIds, string value)
    {
        var operationClaims = user.UserOperationClaims.Where(urc => urc.Value != value).ToList();

        operationClaimIds.ForEach(operationClaimId =>
        {
            var userRoleClaim = UserOperationClaim.Create(user.Id, operationClaimId, value);
            operationClaims.Add(userRoleClaim);
        });

        user.UserOperationClaims = operationClaims;
    }

    public void UpdateUsersWithRolesAndOperationClaims(List<User> users, List<Guid> roleIds,
        List<Guid> operationClaimIds,
        string value)
    {
        users.ForEach(user =>
        {
            this.UpdateRoleClaims(user, roleIds, value);
            this.UpdateOperationClaims(user, operationClaimIds, value);
        });
    }

    public void DeleteStreamModerators(List<User> users, string value)
    {
        users.ForEach(user =>
        {
            user.UserRoleClaims = user.UserRoleClaims.Where(urc => urc.Value != value).ToList();
            user.UserOperationClaims = user.UserOperationClaims.Where(uoc => uoc.Value != value).ToList();
        });
    }
}