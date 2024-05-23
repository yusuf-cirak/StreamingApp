using Application.Common.Mapping;

namespace Application.Features.Users.Services;

public sealed class UserService : IUserService
{
    private readonly IEfRepository _efRepository;

    public UserService(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public Task<List<GetFollowingStreamDto>> GetFollowingStreamsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return _efRepository.StreamFollowerUsers
            .Include(s => s.Streamer)
            .Where(sfu => sfu.UserId == userId)
            .Select(sfu => new GetFollowingStreamDto(sfu.Streamer.ToDto())).ToListAsync(cancellationToken);
    }

    public IEnumerable<GetBlockedStreamDto> GetBlockedStreamsEnumerable(Guid currentUserId)
    {
        return _efRepository.StreamBlockedUsers.Where(sbu => sbu.UserId == currentUserId)
            .Select(sbu => new GetBlockedStreamDto(sbu.Streamer.ToDto())).AsEnumerable();
    }
}