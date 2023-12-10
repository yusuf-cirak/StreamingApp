using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel;
using System.Reflection;

namespace Infrastructure.Persistence.EntityFramework.Contexts;

public class BaseDbContext : DbContext
{
    private IConfiguration Configuration { get; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Streamer> Streamers { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }


    public BaseDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        Configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        var datas = ChangeTracker.Entries<AuditableEntity>();

        foreach (var data in datas)
        {
            _ = data.State switch
            {
                EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                EntityState.Modified => data.Entity.CreatedDate = DateTime.UtcNow,
                _ => DateTime.UtcNow
            };
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}