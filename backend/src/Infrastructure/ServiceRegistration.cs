using Application.Abstractions.Caching;
using Application.Abstractions.Helpers;
using Application.Abstractions.Repository;
using Infrastructure.BackgroundJobs;
using Infrastructure.Helpers.Hashing;
using Infrastructure.Helpers.JWT;
using Infrastructure.Helpers.Security.Encryption;
using Infrastructure.Persistence.EntityFramework.Contexts;
using Infrastructure.Persistence.EntityFramework.Interceptors;
using Infrastructure.Persistence.EntityFramework.Repositories;
using Infrastructure.Services.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SignalR.Hubs.Stream.Client.Abstractions.Services;
using SignalR.Hubs.Stream.Client.Concretes.Services.InMemory;
using SignalR.Hubs.Stream.Server.Abstractions;
using SignalR.Hubs.Stream.Server.Concretes;
using SignalR.Hubs.Stream.Shared;
using SignalR.Hubs.Stream.Shared.InMemory;

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
        services.AddSingleton<IEncryptionHelper, AesEncryptionHelper>();

        // services.AddQuartzBackgroundJob();

        services.AddSignalrServices();
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

    private static void AddSignalrServices(this IServiceCollection services)
    {
        services.AddSingleton<IStreamHubState, InMemoryStreamHubState>();

        services.AddScoped<IStreamHubUserService, InMemoryStreamHubUserService>();
        services.AddScoped<IStreamHubChatRoomService, InMemoryStreamHubChatRoomService>();

        services.AddScoped<IStreamHubClientService, InMemoryStreamHubClientService>();
        services.AddScoped<IStreamHubServerService, InMemoryStreamHubServerService>();

        services.AddSingleton<ICacheService, RedisCacheService>();
    }
}