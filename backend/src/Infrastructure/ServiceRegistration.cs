using Application.Abstractions.Helpers;
using Application.Abstractions.Repository;
using Infrastructure.Helpers.Hashing;
using Infrastructure.Helpers.JWT;
using Infrastructure.Helpers.Security.Encryption;
using Infrastructure.Persistence.EntityFramework.Contexts;
using Infrastructure.Persistence.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEfRepository, EfRepository>();

        services.AddDbContextPool<BaseDbContext>(opt =>
        {
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            opt.UseNpgsql(configuration.GetConnectionString("Postgres"),
                sqlOpt => { sqlOpt.EnableRetryOnFailure(maxRetryCount: 3); });
        }, poolSize: 100);


        services.AddSingleton<IJwtHelper, JwtHelper>();
        services.AddSingleton<IHashingHelper, HashingHelper>();
        services.AddSingleton<IEncryptionHelper, EncryptionHelper>();

    }
}