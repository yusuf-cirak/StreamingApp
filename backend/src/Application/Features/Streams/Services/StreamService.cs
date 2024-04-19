using Application.Abstractions.Caching;
using Application.Common.Constants;
using Application.Common.Mapping;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Stream = Domain.Entities.Stream;

namespace Application.Features.Streams.Services;

public sealed class StreamService : IStreamService
{
    private readonly IEncryptionHelper _encryptionHelper;
    private readonly IEfRepository _efRepository;
    private readonly IRedisDatabase _redisDatabase;
    private readonly ICacheService _cacheService;

    private readonly List<GetStreamDto> _liveStreamers = [];

    public StreamService(IEncryptionHelper encryptionHelper, IEfRepository efRepository,
        IRedisDatabase redisDatabase, ICacheService cacheService)
    {
        _encryptionHelper = encryptionHelper;
        _efRepository = efRepository;
        _redisDatabase = redisDatabase;
        _cacheService = cacheService;
    }


    public async Task<Result<StreamOption, Error>> StreamerExistsAsync(string streamKey,
        CancellationToken cancellationToken = default)
    {
        var streamer = await _efRepository
            .StreamOptions
            .Include(s => s.Streamer)
            .Where(s => s.StreamKey == streamKey)
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (streamer is null)
        {
            return StreamOptionErrors.StreamerDoesNotExist;
        }

        return streamer;
    }

    public async Task<Result> IsStreamerLiveAsync(User user, string streamKey,
        CancellationToken cancellationToken = default)
    {
        var liveStreamers = await this.GetLiveStreamsAsync(cancellationToken);

        var isStreamerLive =
            liveStreamers.Exists(ls => ls.StreamOption!.Value.StreamKey == streamKey || ls.User.Id == user.Id);

        return isStreamerLive ? Result.Failure(StreamErrors.StreamIsLive) : Result.Success();
    }

    public Task<List<GetStreamDto>> GetLiveStreamsAsync(CancellationToken cancellationToken = default)
    {
        return _cacheService.GetOrAddAsync(RedisConstant.Key.LiveStreamers,
            _efRepository.GetLiveStreamers(cancellationToken).AsTask, cancellationToken: cancellationToken);
    }

    public async Task<Result<GetStreamDto, Error>> GetLiveStreamerByNameAsync(string streamerName,
        CancellationToken cancellationToken = default)
    {
        var liveStreams = _liveStreamers.Count == 0 ? await this.GetLiveStreamsAsync(cancellationToken) : [];

        var liveStream = liveStreams.SingleOrDefault(stream => stream.User.Username == streamerName);

        if (liveStream is not null)
        {
            return liveStream;
        }

        var streamerExists = await _efRepository
            .StreamOptions
            .Include(s => s.Streamer)
            .Where(s => s.Streamer.Username == streamerName)
            .AnyAsync(cancellationToken);


        if (!streamerExists)
        {
            return StreamErrors.StreamerNotExists;
        }

        return StreamErrors.StreamIsNotLive;
    }


    public async Task<Result<GetStreamDto, Error>> GetLiveStreamerByKeyAsync(string streamerKey,
        CancellationToken cancellationToken = default)
    {
        var liveStreams = _liveStreamers.Count == 0 ? await this.GetLiveStreamsAsync(cancellationToken) : [];

        var liveStream = liveStreams.SingleOrDefault(stream => stream.StreamOption!.Value.StreamKey == streamerKey);


        if (liveStream is not null)
        {
            return liveStream;
        }

        return StreamErrors.StreamIsNotLive;
    }

    public ValueTask<List<GetFollowingStreamDto>> GetFollowingStreamsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return _efRepository.GetFollowingStreamersAsync(userId, cancellationToken);
    }

    public async Task<bool> StartNewStreamAsync(StreamOption streamOption, Stream stream,
        CancellationToken cancellationToken = default)
    {
        _efRepository.Streams.Add(stream);

        var insertResult = await _efRepository.SaveChangesAsync(cancellationToken);

        if (insertResult == 0)
        {
            return false;
        }

        var getStreamDto = stream.ToDto(streamOption.Streamer.ToDto(), streamOption.ToDto());

        var cacheUpdated = await this.AddNewStreamToCacheAsync(getStreamDto);

        var newStreamKey = GenerateStreamKey(streamOption.Streamer);

        var updateStreamKeyResult = await _efRepository.StreamOptions
            .Where(st => st.Id == streamOption.Streamer.Id)
            .ExecuteUpdateAsync(
                streamer => streamer
                    .SetProperty(x => x.StreamKey, x => newStreamKey),
                cancellationToken:
                cancellationToken);

        return insertResult > 0 && cacheUpdated && updateStreamKeyResult > 0;
    }

    private async Task<bool> AddNewStreamToCacheAsync(GetStreamDto streamDto)
    {
        var liveStreams = _liveStreamers.Count == 0 ? await this.GetLiveStreamsAsync() : [];

        liveStreams.Add(streamDto);

        return await _cacheService.SetAsync(RedisConstant.Key.LiveStreamers, liveStreams);
    }

    public async Task<bool> EndStreamAsync(GetStreamDto stream)
    {
        var removeFromCacheTask = RemoveStreamFromCacheAsync(stream);
        var setEndDateTask = SetStreamEndDateToDatabaseAsync(stream);

        await Task.WhenAll(removeFromCacheTask, setEndDateTask);

        return removeFromCacheTask.Result && setEndDateTask.Result > 0;
    }

    private async Task<bool> RemoveStreamFromCacheAsync(GetStreamDto stream)
    {
        var liveStreams = _liveStreamers.Count == 0 ? await this.GetLiveStreamsAsync() : [];

        liveStreams.Remove(stream);

        return await _cacheService.SetAsync(RedisConstant.Key.LiveStreamers, liveStreams);
    }

    private Task<int> SetStreamEndDateToDatabaseAsync(GetStreamDto streamDto)
    {
        return _efRepository
            .Streams
            .Where(s => s.Id == streamDto.Id)
            .ExecuteUpdateAsync(stream => stream
                .SetProperty(s => s.EndedAt, DateTime.UtcNow));
    }

    public string GenerateStreamKey(User streamer)
    {
        var streamKeyText = $"{streamer.Username}-{GenerateRandomText()}";
        return _encryptionHelper.Encrypt(streamKeyText);
    }

    static string GenerateRandomText()
    {
        const int length = 5;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        Random random = new Random();

        var randomChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            randomChars[i] = chars[random.Next(chars.Length)];
        }

        return new string(randomChars);
    }
}