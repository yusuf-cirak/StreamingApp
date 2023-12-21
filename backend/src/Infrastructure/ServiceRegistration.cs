using Application.Abstractions.Helpers;
using Application.Abstractions.Repository;
using Infrastructure.BackgroundJobs;
using Infrastructure.Helpers.Hashing;
using Infrastructure.Helpers.JWT;
using Infrastructure.Helpers.Security.Encryption;
using Infrastructure.Persistence.EntityFramework.Contexts;
using Infrastructure.Persistence.EntityFramework.Interceptors;
using Infrastructure.Persistence.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEfRepository, EfRepository>();

        services.AddSingleton<AuditableEntityDateInterceptor>();
        services.AddSingleton<DomainEventToOutboxMessageInterceptor>();

        services.AddDbContextPool<BaseDbContext>((sp, opt) =>
        {
            var auditableEntityDateInterceptor = sp.GetRequiredService<AuditableEntityDateInterceptor>();
            var domainEventToOutboxMessageInterceptor = sp.GetRequiredService<DomainEventToOutboxMessageInterceptor>();

            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            opt.UseNpgsql(configuration.GetConnectionString("Postgres"),
                    sqlOpt => { sqlOpt.EnableRetryOnFailure(maxRetryCount: 3); })
                .AddInterceptors(auditableEntityDateInterceptor, domainEventToOutboxMessageInterceptor);
        }, poolSize: 100);


        services.AddSingleton<IJwtHelper, JwtHelper>();
        services.AddSingleton<IHashingHelper, HashingHelper>();
        services.AddSingleton<IEncryptionHelper, EncryptionHelper>();

        services.AddQuartzBackgroundJob();
    }


    private static void AddQuartzBackgroundJob(this IServiceCollection services)
    {
        services.AddQuartz(configurator =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configurator
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger => trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInSeconds(5)
                        .RepeatForever()));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}