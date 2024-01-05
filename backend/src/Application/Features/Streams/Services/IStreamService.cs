using Application.Features.Streams.Dtos;
using Stream = Domain.Entities.Stream;

namespace Application.Features.Streams.Services;

public interface IStreamService : IDomainService<Stream>
{
    Guid GetUserIdFromStreamKey(string streamKey);
    Task<Result<Streamer, Error>> StreamerExistsAsync(Guid streamerId, CancellationToken cancellationToken);
    Task<Result> IsStreamerLiveAsync(Guid streamerId, CancellationToken cancellationToken);

    Task<bool> StartNewStreamAsync(Streamer streamer, Stream stream, CancellationToken cancellationToken);
    Task<List<GetStreamDto>> GetLiveStreamsAsync();

    Task<bool> EndStreamAsync(GetStreamDto stream, CancellationToken cancellationToken);
}