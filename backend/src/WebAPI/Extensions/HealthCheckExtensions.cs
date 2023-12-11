namespace WebAPI.Extensions;

internal static class HealthCheckExtensions
{
    internal static void AddHealthCheckServices(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresConnectionString = configuration.GetConnectionString("Postgres")!;
        services.AddHealthChecks().AddNpgSql(postgresConnectionString);
    }
}