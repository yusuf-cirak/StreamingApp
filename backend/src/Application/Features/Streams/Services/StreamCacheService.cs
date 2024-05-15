﻿using Application.Abstractions.Caching;
using Application.Common.Constants;
using Application.Common.Mapping;

namespace Application.Features.Streams.Services;

public sealed class StreamCacheService : IStreamCacheService
{
    public List<GetStreamDto> LiveStreamers { get; }
    private readonly ICacheService _cacheService;
    private readonly IEfRepository _efRepository;


    public StreamCacheService(IEfRepository efRepository, ICacheService cacheService)
    {
        _efRepository = efRepository;
        _cacheService = cacheService;
        // todo: implement a better solution
        LiveStreamers = this.GetLiveStreamsAsync().GetAwaiter().GetResult();
    }


    private Task<List<GetStreamDto>> GetLiveStreamsAsync(CancellationToken cancellationToken = default)
    {
        return _cacheService.GetOrAddAsync(RedisConstant.Key.LiveStreamers,
            factory: _efRepository.GetLiveStreamersAsync(cancellationToken),
            cancellationToken: cancellationToken);
    }

    public Task<bool> SetLiveStreamsAsync(List<GetStreamDto> liveStreams, CancellationToken cancellationToken = default)
    {
        return _cacheService.SetAsync(RedisConstant.Key.LiveStreamers, liveStreams,
            cancellationToken: cancellationToken);
    }

    public async Task<bool> AddNewStreamToCacheAsync(GetStreamDto streamDto,
        CancellationToken cancellationToken = default)
    {
        LiveStreamers.Add(streamDto);

        return await _cacheService.SetAsync(RedisConstant.Key.LiveStreamers, LiveStreamers,
            cancellationToken: cancellationToken);
    }

    public Task<bool> RemoveStreamFromCacheAsync(int index, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Index is: {index}");
        LiveStreamers.RemoveAt(index);

        return this.SetLiveStreamsAsync(LiveStreamers, cancellationToken);
    }

    private Func<Task<List<GetStreamDto>>> GetLiveStreamersFromDatabaseAsync(
        CancellationToken cancellationToken = default)
    {
        return () => _efRepository
            .Streams
            .AsTracking()
            .Include(s => s.Streamer.Streams)
            .Include(s => s.Streamer.StreamOption)
            .Where(s => s.EndedAt == default)
            .Select(MappingExtensions.ToGetStreamDto)
            .ToListAsync(cancellationToken);
    }
}