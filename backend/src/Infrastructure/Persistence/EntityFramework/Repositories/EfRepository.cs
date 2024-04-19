using Application.Abstractions.Repository;
using Application.Common.Models;
using Application.Contracts.Streams;
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
    public DbSet<StreamOption> StreamOptions => Context.StreamOptions;
    public DbSet<RefreshToken> RefreshTokens => Context.RefreshTokens;
    public DbSet<OperationClaim> OperationClaims => Context.OperationClaims;
    public DbSet<RoleOperationClaim> RoleOperationClaims => Context.RoleOperationClaims;
    public DbSet<UserRoleClaim> UserRoleClaims => Context.UserRoleClaims;
    public DbSet<StreamFollowerUser> StreamFollowerUsers => Context.StreamFollowerUsers;
    public DbSet<StreamBlockedUser> StreamBlockedUsers => Context.StreamBlockedUsers;

    public DbSet<OutboxMessage> OutboxMessages => Context.OutboxMessages;

    public DbSet<Role> Roles => Context.Roles;

    public DbSet<UserOperationClaim> UserOperationClaims => Context.UserOperationClaims;


    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Context.SaveChangesAsync(cancellationToken);
    }


    public ValueTask<List<GetStreamDto>> GetLiveStreamers(CancellationToken cancellationToken = default)
    {
        return CompiledQueries.GetLiveStreamers(Context).ToListAsync(cancellationToken);
    }

    public ValueTask<List<GetFollowingStreamDto>> GetFollowingStreamersAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return CompiledQueries.GetFollowingStreamers(Context, userId).ToListAsync(cancellationToken);
    }
}