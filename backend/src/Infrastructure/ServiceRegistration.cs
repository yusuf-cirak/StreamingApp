using Application.Abstractions.Helpers;
using Infrastructure.Helpers.Hashing;
using Infrastructure.Helpers.JWT;
using Infrastructure.Persistence.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContextPool<BaseDbContext>(opt =>
        {
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            opt.UseNpgsql(configuration.GetConnectionString("Postgres"), sqlOpt =>
            {
                sqlOpt.EnableRetryOnFailure(maxRetryCount: 3);

            });

        }, poolSize: 100);


        services.AddSingleton<IJwtHelper, JwtHelper>();
        services.AddSingleton<IHashingHelper, HashingHelper>();

    }
}