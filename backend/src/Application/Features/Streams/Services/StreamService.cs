using Stream = Domain.Entities.Stream;

namespace Application.Features.Streams.Services;

public sealed class StreamService : IStreamService
{
    private readonly IEncryptionHelper _encryptionHelper;
    private readonly IEfRepository _efRepository;

    public StreamService(IEncryptionHelper encryptionHelper, IEfRepository efRepository)
    {
        _encryptionHelper = encryptionHelper;
        _efRepository = efRepository;
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


    public async Task<int> StartNewStreamAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        _efRepository.Streams.Add(stream);


        var insertResult = await _efRepository.SaveChangesAsync(cancellationToken);

        if (insertResult == 0)
        {
            return 0;
        }

        // TODO: Add started stream to Redis cache

        return insertResult;
    }

    private string GetStreamKeyFromUserId(Guid userId)
    {
        return _encryptionHelper.Encrypt(userId.ToString());
    }
}