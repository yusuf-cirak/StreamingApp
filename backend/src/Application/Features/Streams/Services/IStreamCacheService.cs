namespace Application.Features.Streams.Services;

public interface IStreamCacheService
{
    Task<List<GetStreamDto>> GetLiveStreamsAsync(CancellationToken cancellationToken = default);
    
    Task<bool> SetLiveStreamsAsync(List<GetStreamDto> liveStreams,CancellationToken cancellationToken=default);

    Task<bool> AddNewStreamToCacheAsync(GetStreamDto streamDto);

    Task<bool> RemoveStreamFromCacheAsync(GetStreamDto stream);
    

}