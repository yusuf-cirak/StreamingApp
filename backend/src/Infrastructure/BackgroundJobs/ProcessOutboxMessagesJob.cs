using Infrastructure.Persistence.EntityFramework.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using SharedKernel;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessagesJob : IJob
{
    private readonly BaseDbContext _dbContext;
    private readonly IPublisher _publisher;

    public ProcessOutboxMessagesJob(BaseDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var outboxMessages = await _dbContext.OutboxMessages
            .Where(x => x.ProcessedOnUtc == null)
            .ToListAsync(context.CancellationToken);

        foreach (var outboxMessage in outboxMessages)
        {
            var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content);

            await _publisher.Publish(domainEvent);

            outboxMessage.MarkAsProcessed();
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}