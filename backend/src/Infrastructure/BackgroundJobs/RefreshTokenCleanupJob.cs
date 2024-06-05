using Application.Abstractions.Repository;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Infrastructure.BackgroundJobs;

public sealed class RefreshTokenCleanupJob(IEfRepository efRepository) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var count = await efRepository
            .RefreshTokens
            .Where(rt => rt.ExpiresAt < DateTime.UtcNow)
            .ExecuteDeleteAsync(context.CancellationToken);


        Console.WriteLine($"Refresh tokens cleaned up. Count : {count}");
    }
}