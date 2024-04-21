using Application.Common.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel;

namespace Infrastructure.Persistence.EntityFramework.Interceptors;

public sealed class DomainEventToOutboxMessageInterceptor : SaveChangesInterceptor
{
    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            throw new ArgumentNullException(nameof(dbContext));
        }

        var domainEvents = dbContext.ChangeTracker.Entries<Entity>()
            .Select(x => x.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            });


        var outboxMessages = domainEvents
            .Select(OutboxMessage.Create);

        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}