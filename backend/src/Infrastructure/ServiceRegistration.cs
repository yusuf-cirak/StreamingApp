using Application.Abstractions.Caching;
using Application.Abstractions.Helpers;
using Application.Abstractions.Image;
using Application.Abstractions.Repository;
using CloudinaryDotNet;
using Infrastructure.BackgroundJobs;
using Infrastructure.Helpers.Hashing;
using Infrastructure.Helpers.JWT;
using Infrastructure.Helpers.Security.Encryption;
using Infrastructure.Persistence.EntityFramework.Contexts;
using Infrastructure.Persistence.EntityFramework.Interceptors;
using Infrastructure.Persistence.EntityFramework.Repositories;
using Infrastructure.Services.Cache;
using Infrastructure.Services.Image;
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
        services.AddSingleton<IEncryptionHelper, AesEncryptionHelper>();

        services.AddQuartzBackgroundJob();

        services.AddSingleton<ICacheService, RedisCacheService>();

        services.AddCacheServices();

        services.AddImageServices(configuration);
    }


    private static void AddQuartzBackgroundJob(this IServiceCollection services)
    {
        services.AddQuartz(configurator =>
        {
            // var processOutboxMessagesKey = new JobKey(nameof(ProcessOutboxMessagesJob));
            //
            // configurator
            //     .AddJob<ProcessOutboxMessagesJob>(processOutboxMessagesKey)
            //     .AddTrigger(trigger => trigger
            //         .ForJob(processOutboxMessagesKey)
            //         .WithSimpleSchedule(schedule => schedule
            //             .WithIntervalInSeconds(5)
            //             .RepeatForever()));


            var refreshTokenCleanupKey = new JobKey(nameof(RefreshTokenCleanupJob));


            configurator
                .AddJob<RefreshTokenCleanupJob>(refreshTokenCleanupKey)
                .AddTrigger(trigger => trigger
                    .ForJob(refreshTokenCleanupKey)
                    .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInHours(24)
                        .RepeatForever()));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }

    private static void AddCacheServices(this IServiceCollection services)
    {
        services.AddScoped<ICacheService, RedisCacheService>();
    }


    private static void AddImageServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(_ =>
            new Cloudinary(account: configuration.GetSection("CloudinarySettings").Get<Account>()));

        services.AddScoped<IImageService, CloudinaryImageService>();
    }
}