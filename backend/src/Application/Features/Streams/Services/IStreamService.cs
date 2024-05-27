using Stream = Domain.Entities.Stream;

namespace Application.Features.Streams.Services;

public interface IStreamService : IDomainService<Stream>
{
    IAsyncEnumerable<GetStreamDto> GetRecommendedStreamersAsyncEnumerable();

    Task<Result<StreamOption, Error>> StreamerExistsAsync(string streamKey,
        CancellationToken cancellationToken = default);

    Result IsStreamerLive(User user, string streamKey);

    Task<GetStreamInfoDto> GetLiveStreamerByNameAsync(string streamerName,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GetStreamDto> FindStreamersByNameAsyncEnumerable(string term);

    Result<(GetStreamDto, int), Error> GetLiveStreamerByKeyFromCache(string streamerKey,
        CancellationToken cancellationToken = default);

    Task<bool> StartNewStreamAsync(StreamOption streamOption, Stream stream,
        CancellationToken cancellationToken = default);

    Task<bool> EndStreamAsync(int index, CancellationToken cancellationToken = default);

    Task AddToCacheAndSendNotificationAsync(GetStreamDto streamDto);

    Task<bool> AddToCacheAsync(GetStreamDto streamDto);

    string GenerateStreamKey(User user);
}