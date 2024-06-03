using System.Text.Json;
using SharedKernel;

namespace Application.Contracts.Common.Models;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime OccuredOnUtc { get; set; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }

    private OutboxMessage()
    {
    }

    private OutboxMessage(IDomainEvent domainEvent)
    {
        Id = Guid.NewGuid();
        OccuredOnUtc = DateTime.UtcNow;
        Type = domainEvent.GetType().Name;
        Content = JsonSerializer.Serialize(domainEvent);
    }

    public static OutboxMessage Create(IDomainEvent domainEvent) => new(domainEvent);

    public void MarkAsProcessed() => ProcessedOnUtc = DateTime.UtcNow;

    public void MarkAsFailed(string error) => Error = error;
}