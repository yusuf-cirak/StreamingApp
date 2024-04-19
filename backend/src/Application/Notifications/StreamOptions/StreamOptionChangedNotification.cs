using Application.Common.Mapping;
using Application.Features.Streams.Services;
using Domain.Events;

namespace Application.Notifications.StreamOptions;

public sealed class StreamOptionChangedNotification : INotificationHandler<StreamOptionUpdatedEvent>
{
    private readonly IStreamCacheService _streamCacheService;

    public StreamOptionChangedNotification(IStreamCacheService streamCacheService)
    {
        _streamCacheService = streamCacheService;
    }

    public async Task Handle(StreamOptionUpdatedEvent notification, CancellationToken cancellationToken)
    {
        List<GetStreamDto> liveStreamers = await _streamCacheService.GetLiveStreamsAsync(cancellationToken);

        var index = liveStreamers.FindIndex(ls => ls.Id == notification.StreamOption.Streamer.Id);

        if (index is -1)
        {
            return;
        }

        liveStreamers[index].StreamOption = notification.StreamOption.ToDto();

        await _streamCacheService.SetLiveStreamsAsync(liveStreamers, cancellationToken);
    }
}