using Application.Common.Mapping;
using Application.Features.Streams.Dtos;
using Application.Features.Streams.Services;
using SignalR.Hubs.Stream.Server.Abstractions;

namespace Application.Features.StreamBlockedUsers.Services;

public sealed class StreamBlockUserService : IStreamBlockUserService
{
    private readonly IEfRepository _efRepository;

    private readonly IStreamCacheService _streamCacheService;
    private readonly IStreamHubServerService _streamHubServerService;

    public StreamBlockUserService(IEfRepository efRepository, IStreamCacheService streamCacheService,
        IStreamHubServerService streamHubServerService)
    {
        _efRepository = efRepository;
        _streamCacheService = streamCacheService;
        _streamHubServerService = streamHubServerService;
    }

    public Task<int> BlockUserFromStreamAsync(Guid streamerId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        var streamBlockedUser = StreamBlockedUser.Create(streamerId, userId);

        _efRepository.StreamBlockedUsers.Add(streamBlockedUser);

        // todo: use unit of work
        return _efRepository.SaveChangesAsync(cancellationToken);
    }

    public Task<int> UnblockUsersFromStreamAsync(Guid streamerId, List<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        return _efRepository
            .StreamBlockedUsers
            .Where(sbu => sbu.StreamerId == streamerId && userIds.Contains(sbu.UserId))
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<bool> IsUserBlockedFromStreamAsync(Guid streamerId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        var isBlocked = await _efRepository
            .StreamBlockedUsers
            .AnyAsync(sbu => sbu.StreamerId == streamerId && sbu.UserId == userId, cancellationToken);

        return isBlocked;
    }

    public async Task SendBlockNotificationToUsersAsync(Guid streamerId, List<Guid> userIds, bool isBlocked)
    {
        var streamerDto = _streamCacheService.LiveStreamers.SingleOrDefault(ls => ls.User.Id == streamerId)?.User ??
                          await _efRepository.Users.Where(u => u.Id == streamerId).Select(u => u.ToDto())
                              .SingleOrDefaultAsync();

        if (streamerDto is null)
        {
            return;
        }

        await _streamHubServerService.OnBlockFromStreamAsync(streamerDto, userIds, isBlocked);
    }

    public IAsyncEnumerable<GetStreamBlockedUserDto> GetBlockedUsersOfStreamAsyncEnumerable(Guid streamerId)
    {
        return _efRepository
            .StreamBlockedUsers
            .Include(sbu => sbu.User)
            .Where(sbu => sbu.StreamerId == streamerId)
            .Select(sbu => sbu.User.ToStreamBlockedUserDto(sbu.CreatedDate))
            .AsAsyncEnumerable();
    }
}