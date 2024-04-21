using Infrastructure.Persistence.EntityFramework.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quartz;
using SharedKernel;
using Newtonsoft.Json;

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
        var outboxMessages = _dbContext.OutboxMessages
            .AsTracking()
            .Where(x => x.ProcessedOnUtc == null);

        foreach (var outboxMessage in outboxMessages)
        {
            var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content);

            await _publisher.Publish(domainEvent);

            outboxMessage.MarkAsProcessed();
        }

        if (outboxMessages.Any())
        {
            await _dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}