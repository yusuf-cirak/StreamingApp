using Application.Abstractions.Caching;

namespace Application.Features.Users.Services;

public interface IUserBlacklistManager : IDomainService<User>
{
    Task<bool> IsUserBlacklistedAsync(string userId);
    Task<bool> AddUserToBlacklistAsync(string userId);

    Task AddUsersToBlacklistAsync(List<string> userIds);
    Task<bool> RemoveUserFromBlacklistAsync(string userId);

    Task RemoveUsersFromBlacklistAsync(List<string> userIds);
}

public sealed class UserBlacklistManager : IUserBlacklistManager
{
    private const string UserBlacklist = "user-blacklist";
    private readonly IRedisCacheService _redisCacheService;

    public UserBlacklistManager(IRedisCacheService redisCacheService)
    {
        _redisCacheService = redisCacheService;
    }

    public Task<bool> IsUserBlacklistedAsync(string userId)
        => _redisCacheService
            .RedisDb
            .HashExistsAsync(UserBlacklist, userId);

    public Task<bool> AddUserToBlacklistAsync(string userId)
        => _redisCacheService
            .RedisDb
            .HashSetAsync(UserBlacklist, userId, true);

    public Task AddUsersToBlacklistAsync(List<string> userIds)
        => _redisCacheService
            .RedisDb
            .HashSetAsync(UserBlacklist, userIds.ToDictionary(key => key, _ => true));


    public Task<bool> RemoveUserFromBlacklistAsync(string userId)
        => _redisCacheService
            .RedisDb
            .HashDeleteAsync(UserBlacklist, userId);

    public Task RemoveUsersFromBlacklistAsync(List<string> userIds)
        => _redisCacheService
            .RedisDb
            .HashDeleteAsync(UserBlacklist, userIds.ToArray());
}