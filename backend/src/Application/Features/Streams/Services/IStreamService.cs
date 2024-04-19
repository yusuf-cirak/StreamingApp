using Application.Contracts.Streams;
using Stream = Domain.Entities.Stream;

namespace Application.Features.Streams.Services;

public interface IStreamService : IDomainService<Stream>
{
    Task<Result<StreamOption, Error>> StreamerExistsAsync(string streamKey,
        CancellationToken cancellationToken = default);

    Task<Result> IsStreamerLiveAsync(User user, string streamKey, CancellationToken cancellationToken = default);

    Task<Result<GetStreamDto, Error>> GetLiveStreamerByKeyAsync(string streamerKey,
        CancellationToken cancellationToken = default);

    Task<bool> StartNewStreamAsync(StreamOption streamOption, Stream stream,
        CancellationToken cancellationToken = default);

    Task<List<GetStreamDto>> GetLiveStreamsAsync(CancellationToken cancellationToken = default);

    Task<Result<GetStreamDto, Error>> GetLiveStreamerByNameAsync(string streamerName,
        CancellationToken cancellationToken = default);

    ValueTask<List<GetFollowingStreamDto>> GetFollowingStreamsAsync(Guid userId,
        CancellationToken cancellationToken = default);

    Task<bool> EndStreamAsync(GetStreamDto stream);
    string GenerateStreamKey(User user);
}