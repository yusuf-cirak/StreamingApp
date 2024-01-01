using Stream = Domain.Entities.Stream;

namespace Application.Features.Streams.Services;

public interface IStreamService : IDomainService<Stream>
{
    Guid GetUserIdFromStreamKey(string streamKey);
    Task<bool> StreamerExistsAsync(Guid streamerId, CancellationToken cancellationToken);
    Task<int> StartNewStreamAsync(Stream stream, CancellationToken cancellationToken);
}