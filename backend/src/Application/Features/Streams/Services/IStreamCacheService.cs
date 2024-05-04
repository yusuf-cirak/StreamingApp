namespace Application.Features.Streams.Services;

public interface IStreamCacheService
{
    public List<GetStreamDto> LiveStreamers { get; }
    Task<List<GetStreamDto>> GetLiveStreamsAsync(CancellationToken cancellationToken = default);

    Task<bool> SetLiveStreamsAsync(List<GetStreamDto> liveStreams, CancellationToken cancellationToken = default);

    Task<bool> AddNewStreamToCacheAsync(GetStreamDto streamDto, CancellationToken cancellationToken = default);

    Task<bool> RemoveStreamFromCacheAsync(int index, CancellationToken cancellationToken = default);
}