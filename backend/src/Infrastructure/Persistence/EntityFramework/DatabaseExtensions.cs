using Infrastructure.Persistence.EntityFramework.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.EntityFramework;

public static class DatabaseExtensions
{
    public static void ApplyPendingMigrations(this IApplicationBuilder app)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
        using var context = serviceScope.ServiceProvider.GetRequiredService<BaseDbContext>();
        context.Database.Migrate();
    }

    public static void GenerateSeedDataAndPersist(this IApplicationBuilder app)
    {
        IServiceProvider services = app.ApplicationServices.CreateScope().ServiceProvider;
        var seedDataGenerator = new SeedDataGenerator(services);
        seedDataGenerator.GenerateSeedDataAndPersist();
    }
}