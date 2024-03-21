using Application.Common.Constants;
using Application.Common.Mapping;
using Application.Features.Streams.Dtos;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Stream = Domain.Entities.Stream;

namespace Application.Features.Streams.Services;

public sealed class StreamService : IStreamService
{
    private readonly IEncryptionHelper _encryptionHelper;
    private readonly IEfRepository _efRepository;
    private readonly IRedisDatabase _redisDatabase;

    public StreamService(IEncryptionHelper encryptionHelper, IEfRepository efRepository,
        IRedisDatabase redisDatabase)
    {
        _encryptionHelper = encryptionHelper;
        _efRepository = efRepository;
        _redisDatabase = redisDatabase;
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

    public async Task<Result> IsStreamerLiveAsync(User user, string streamKey, CancellationToken cancellationToken)
    {
        var liveStreamers = (await _redisDatabase.Database.ListRangeAsync(RedisConstant.Key.LiveStreamers))
            .Select(ls => _redisDatabase.Serializer.Deserialize<GetStreamDto>(ls)).ToList();

        var isStreamerLive =
            liveStreamers.Exists(ls => ls.StreamOption!.Value.StreamKey == streamKey || ls.User.Id == user.Id);

        return isStreamerLive ? Result.Failure(StreamErrors.StreamIsLive) : Result.Success();
    }

    public async Task<List<GetStreamDto>> GetLiveStreamsAsync()
    {
        var liveStreams = (await _redisDatabase.Database.ListRangeAsync(RedisConstant.Key.LiveStreamers))
            .Select(ls => _redisDatabase.Serializer.Deserialize<GetStreamDto>(ls)).ToList();

        return liveStreams;
    }

    public async Task<Result<GetStreamDto, Error>> GetLiveStreamerByNameAsync(string streamerName)
    {
        var liveStream = (await _redisDatabase.Database.ListRangeAsync(RedisConstant.Key.LiveStreamers))
            .Select(ls => _redisDatabase.Serializer.Deserialize<GetStreamDto>(ls))
            .FirstOrDefault(stream => stream.User.Username == streamerName);


        if (liveStream is not null){
            return liveStream;
        }

        var streamerExists = await _efRepository
            .StreamOptions
            .Include(s => s.Streamer)
            .Where(s => s.Streamer.Username == streamerName)
            .AnyAsync();


        if (!streamerExists)
        {
            return StreamErrors.StreamerNotExists;
        }

        return StreamErrors.StreamIsNotLive;
    }


        public async Task<Result<GetStreamDto, Error>> GetLiveStreamerByKeyAsync(string streamerKey)
    {
        var liveStream = (await _redisDatabase.Database.ListRangeAsync(RedisConstant.Key.LiveStreamers))
            .Select(ls => _redisDatabase.Serializer.Deserialize<GetStreamDto>(ls))
            .FirstOrDefault(stream => stream.StreamOption.Value.StreamKey == streamerKey);


        if (liveStream is not null) {
            return liveStream;
        }

        return StreamErrors.StreamIsNotLive;
    }

    public Task<List<GetFollowingStreamDto>> GetFollowingStreamsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var followingStreams = _efRepository
            .StreamFollowerUsers
            .Include(sfu => sfu.Streamer)
            .Where(sfu => sfu.UserId == userId)
            .Select(sfu => new GetFollowingStreamDto(sfu.Streamer.ToDto()))
            .ToListAsync(cancellationToken: cancellationToken);

        return followingStreams;
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

        var getStreamDto = stream.ToDto(stream.Id, streamOption.Streamer.ToDto(), streamOption.ToStreamOptionDto());

        var serializedStream = _redisDatabase.Serializer.Serialize(getStreamDto);

        var redisIndex = await _redisDatabase.Database.ListRightPushAsync(RedisConstant.Key.LiveStreamers,
            serializedStream);


        var newStreamKey = GenerateStreamKey(streamOption.Streamer);

        var updateStreamKeyResult = await _efRepository.StreamOptions
            .Where(st => st.Id == streamOption.Streamer.Id)
            .ExecuteUpdateAsync(
                streamer => streamer
                    .SetProperty(x => x.StreamKey, x => newStreamKey),
                cancellationToken:
                cancellationToken);

        return insertResult > 0 && redisIndex > 0;
    }

    public async Task<bool> EndStreamAsync(GetStreamDto stream, CancellationToken cancellationToken = default)
    {
        var removeFromCacheTask = RemoveStreamFromCacheAsync(stream);
        var setEndDateTask = SetStreamEndDateToDatabaseAsync(stream, cancellationToken);

        await Task.WhenAll(removeFromCacheTask, setEndDateTask);

        return removeFromCacheTask.Result > -1 && setEndDateTask.Result > 0;
    }

    private async Task<long> RemoveStreamFromCacheAsync(GetStreamDto stream)
    {
        var serializedStream = _redisDatabase.Serializer.Serialize(stream);

        return await _redisDatabase.Database.ListRemoveAsync(RedisConstant.Key.LiveStreamers, serializedStream);
    }

    private async Task<int> SetStreamEndDateToDatabaseAsync(GetStreamDto streamDto,
        CancellationToken cancellationToken = default)
    {
        return await _efRepository
            .Streams
            .Where(s => s.Id == streamDto.Id)
            .ExecuteUpdateAsync(stream => stream
                .SetProperty(s => s.EndedAt, DateTime.UtcNow), cancellationToken);
    }

    public string GenerateStreamKey(User streamer)
    {
        var streamKeyText = $"{streamer.Username}-{DateTime.Now:dd-MM-YYYY:hh:mm:ss}";
        return _encryptionHelper.Encrypt(streamKeyText);
    }
}