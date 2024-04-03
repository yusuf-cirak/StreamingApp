using Application.Contracts.Streams;
using Stream = Domain.Entities.Stream;

namespace Application.Features.Streams.Services;

public interface IStreamService : IDomainService<Stream>
{
    Task<Result<StreamOption, Error>> StreamerExistsAsync(string streamKey, CancellationToken cancellationToken);
    Task<Result> IsStreamerLiveAsync(User user, string streamKey, CancellationToken cancellationToken);
    Task<Result<GetStreamDto, Error>> GetLiveStreamerByKeyAsync(string streamerKey);

    Task<bool> StartNewStreamAsync(StreamOption streamOption, Stream stream, CancellationToken cancellationToken);
    Task<List<GetStreamDto>> GetLiveStreamsAsync();
    Task<Result<GetStreamDto, Error>> GetLiveStreamerByNameAsync(string streamerName);
    Task<List<GetFollowingStreamDto>> GetFollowingStreamsAsync(Guid userId, CancellationToken cancellationToken);

    Task<bool> EndStreamAsync(GetStreamDto stream, CancellationToken cancellationToken);
    string GenerateStreamKey(User user);
}