using Application.Common.Constants;
using Application.Common.Mapping;
using Application.Features.Streamers.Dtos;
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


    public async Task<Result<Streamer, Error>> StreamerExistsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var streamer = await _efRepository
            .Streamers
            .Include(s => s.User)
            .Where(s => s.Id == userId)
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (streamer is null)
        {
            return StreamerErrors.StreamerDoesNotExist;
        }

        return streamer;
    }

    public async Task<Result> IsStreamerLiveAsync(Guid streamerId, CancellationToken cancellationToken)
    {
        var liveStreamers = (await _redisDatabase.Database.ListRangeAsync(RedisConstant.Key.LiveStreamers))
            .Select(ls => _redisDatabase.Serializer.Deserialize<GetStreamDto>(ls)).ToList();

        var isStreamerLive = liveStreamers.Exists(ls => ls.User.Id == streamerId);

        return isStreamerLive ? Result.Failure(StreamErrors.StreamIsLive) : Result.Success();
    }

    public async Task<List<GetStreamDto>> GetLiveStreamsAsync()
    {
        var liveStreams = (await _redisDatabase.Database.ListRangeAsync(RedisConstant.Key.LiveStreamers))
            .Select(ls => _redisDatabase.Serializer.Deserialize<GetStreamDto>(ls)).ToList();

        return liveStreams;
    }

    public Guid GetUserIdFromStreamKey(string streamKey)
    {
        return Guid.Parse(_encryptionHelper.Decrypt(streamKey));
    }


    public async Task<bool> StartNewStreamAsync(Streamer streamer, Stream stream,
        CancellationToken cancellationToken = default)
    {
        _efRepository.Streams.Add(stream);

        var insertResult = await _efRepository.SaveChangesAsync(cancellationToken);

        if (insertResult == 0)
        {
            return false;
        }

        var getStreamDto = stream.ToDto(streamer.User.ToDto(), streamer.ToDto());

        var serializedStream = _redisDatabase.Serializer.Serialize(getStreamDto);

        var index = await _redisDatabase.Database.ListRightPushAsync(RedisConstant.Key.LiveStreamers,
            serializedStream);

        return insertResult > 0 && index > 0;
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

    private string GetStreamKeyFromUserId(Guid userId)
    {
        return _encryptionHelper.Encrypt(userId.ToString());
    }
}