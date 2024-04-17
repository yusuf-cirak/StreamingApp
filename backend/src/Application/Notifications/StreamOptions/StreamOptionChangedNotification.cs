using Domain.Events;

namespace Application.Notifications.StreamOptions;

public class StreamOptionChangedNotification:INotificationHandler<StreamOptionUpdatedEvent>
{
    public Task Handle(StreamOptionUpdatedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}