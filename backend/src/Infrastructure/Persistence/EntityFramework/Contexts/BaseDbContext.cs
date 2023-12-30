using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Application.Common.Models;
using Stream = Domain.Entities.Stream;

namespace Infrastructure.Persistence.EntityFramework.Contexts;

public class BaseDbContext : DbContext
{
    protected IConfiguration Configuration { get; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Stream> Streams { get; set; }
    public virtual DbSet<Streamer> Streamers { get; set; }
    
    public virtual DbSet<StreamChatMessage> StreamChatMessages { get; set; }
    public virtual DbSet<StreamBlockedUser> StreamBlockedUsers { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<OperationClaim> OperationClaims { get; set; }

    public virtual DbSet<UserRoleClaim> UserRoleClaims { get; set; }
    
    public virtual DbSet<UserOperationClaim> UserOperationClaims { get; set; }

    public virtual DbSet<RoleOperationClaim> RoleOperationClaims { get; set; }
    
    public virtual DbSet<OutboxMessage> OutboxMessages { get; set; }
    
    public virtual DbSet<Role> Roles { get; set; }

    public BaseDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        Configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}