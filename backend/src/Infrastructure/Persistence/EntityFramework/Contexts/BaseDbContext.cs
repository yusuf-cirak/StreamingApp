using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel;
using System.Reflection;
using Application.Common.Exceptions;
using MediatR;

namespace Infrastructure.Persistence.EntityFramework.Contexts;

public class BaseDbContext : DbContext
{
    private IPublisher Publisher { get; }
    protected IConfiguration Configuration { get; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Streamer> Streamers { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<OperationClaim> OperationClaims { get; set; }

    public virtual DbSet<UserRoleClaim> UserRoleClaims { get; set; }

    public virtual DbSet<RoleOperationClaim> RoleOperationClaims { get; set; }


    public BaseDbContext(DbContextOptions options, IConfiguration configuration, IPublisher mediator) : base(options)
    {
        Configuration = configuration;
        Publisher = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var auditableEntities = ChangeTracker.Entries<AuditableEntity>();

        foreach (var data in auditableEntities)
        {
            _ = data.State switch
            {
                EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                EntityState.Modified => data.Entity.CreatedDate = DateTime.UtcNow,
                _ => DateTime.UtcNow
            };
        }

        var saveResult = await base.SaveChangesAsync(cancellationToken);

        if (saveResult is 0)
        {
            throw new DatabaseOperationFailedException("Could not save changes to database");
        }

        return saveResult;
    }
}