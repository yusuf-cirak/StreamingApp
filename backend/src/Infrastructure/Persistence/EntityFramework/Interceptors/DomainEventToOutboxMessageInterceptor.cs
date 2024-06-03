using Application.Contracts.Common.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel;
namespace Infrastructure.Persistence.EntityFramework.Interceptors;

public sealed class DomainEventToOutboxMessageInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
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
            .Select(OutboxMessage.Create).ToList();

        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}