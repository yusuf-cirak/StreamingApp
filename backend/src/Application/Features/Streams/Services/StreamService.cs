using Application.Common.Constants;
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


    public async Task<bool> StreamerExistsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var streamerExists = await _efRepository.Streamers.AnyAsync(x => x.Id == userId, cancellationToken);

        return streamerExists;
    }

    public Guid GetUserIdFromStreamKey(string streamKey)
    {
        return Guid.Parse(_encryptionHelper.Decrypt(streamKey));
    }


    public async Task<bool> StartNewStreamAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        _efRepository.Streams.Add(stream);

        var insertResult = await _efRepository.SaveChangesAsync(cancellationToken);

        if (insertResult == 0)
        {
            return false;
        }

        var serializedStream = _redisDatabase.Serializer.Serialize(stream);

        var index = await _redisDatabase.Database.ListRightPushAsync(RedisConstant.Key.LiveStreamers, serializedStream);

        return insertResult > 0 && index > 0;
    }

    private string GetStreamKeyFromUserId(Guid userId)
    {
        return _encryptionHelper.Encrypt(userId.ToString());
    }
}