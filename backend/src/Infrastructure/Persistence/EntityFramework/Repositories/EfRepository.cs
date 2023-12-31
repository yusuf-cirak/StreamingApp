using Application.Abstractions.Repository;
using Application.Common.Models;
using Infrastructure.Persistence.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Stream = Domain.Entities.Stream;

namespace Infrastructure.Persistence.EntityFramework.Repositories;

public sealed class EfRepository : IEfRepository
{
    private BaseDbContext Context { get; }


    public EfRepository(BaseDbContext context)
    {
        Context = context;
    }

    public DbSet<User> Users => Context.Users;
    public DbSet<Stream> Streams => Context.Streams;
    public DbSet<Streamer> Streamers => Context.Streamers;
    public DbSet<RefreshToken> RefreshTokens => Context.RefreshTokens;
    public DbSet<OperationClaim> OperationClaims => Context.OperationClaims;
    public DbSet<RoleOperationClaim> RoleOperationClaims => Context.RoleOperationClaims;
    public DbSet<UserRoleClaim> UserRoleClaims => Context.UserRoleClaims;
    public DbSet<StreamFollowerUser> StreamFollowerUsers => Context.StreamFollowerUsers;
    public DbSet<StreamChatMessage> StreamChatMessages => Context.StreamChatMessages;
    public DbSet<StreamBlockedUser> StreamBlockedUsers => Context.StreamBlockedUsers;

    public DbSet<OutboxMessage> OutboxMessages => Context.OutboxMessages;
    
    public DbSet<Role> Roles => Context.Roles;
    
    public DbSet<UserOperationClaim> UserOperationClaims => Context.UserOperationClaims;



    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await Context.SaveChangesAsync(cancellationToken);
    }

}
