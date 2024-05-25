using Application.Common.Mapping;
using Application.Features.Streams.Services;

namespace Application.Features.Users.Services;

public sealed class UserService : IUserService
{
    private readonly IEfRepository _efRepository;
    private readonly IStreamCacheService _cacheService;

    public UserService(IEfRepository efRepository, IStreamCacheService cacheService)
    {
        _efRepository = efRepository;
        _cacheService = cacheService;
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
}