using Application.Abstractions.Caching;
using Application.Common.Constants;

namespace Application.Features.Streams.Services;

public sealed class StreamCacheService : IStreamCacheService
{
    private readonly ICacheService _cacheService;
    private readonly IEfRepository _efRepository;


    public StreamCacheService(IEfRepository efRepository, ICacheService cacheService)
    {
        _efRepository = efRepository;
        _cacheService = cacheService;
    }

    public Task<List<GetStreamDto>> GetLiveStreamsAsync(CancellationToken cancellationToken = default)
    {
        return _cacheService.GetOrAddAsync(RedisConstant.Key.LiveStreamers,
            factory: _efRepository.GetLiveStreamers(cancellationToken).AsTask, cancellationToken: cancellationToken);
    }

    public Task<bool> SetLiveStreamsAsync(List<GetStreamDto> liveStreams, CancellationToken cancellationToken = default)
    {
        return _cacheService.SetAsync(RedisConstant.Key.LiveStreamers, liveStreams,
            cancellationToken: cancellationToken);
    }

    public async Task<bool> AddNewStreamToCacheAsync(GetStreamDto streamDto)
    {
        var liveStreams = await this.GetLiveStreamsAsync();

        liveStreams.Add(streamDto);

        return await _cacheService.SetAsync(RedisConstant.Key.LiveStreamers, liveStreams);
    }

    public async Task<bool> RemoveStreamFromCacheAsync(GetStreamDto stream)
    {
        var liveStreams = await this.GetLiveStreamsAsync();

        liveStreams.Remove(stream);

        return await _cacheService.SetAsync(RedisConstant.Key.LiveStreamers, liveStreams);
    }
}