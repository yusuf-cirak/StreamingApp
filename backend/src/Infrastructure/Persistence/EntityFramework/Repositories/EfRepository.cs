using Application.Abstractions.Repository;
using Domain.Entities;
using Infrastructure.Persistence.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityFramework.Repositories;

public sealed class EfRepository : IEfRepository
{
    private BaseDbContext Context { get; }


    public EfRepository(BaseDbContext context)
    {
        Context = context;
    }

    public DbSet<User> Users => Context.Users;
    public DbSet<Streamer> Streamers => Context.Streamers;
    public DbSet<RefreshToken> RefreshTokens => Context.RefreshTokens;
    public DbSet<OperationClaim> OperationClaims => Context.OperationClaims;
    public DbSet<RoleOperationClaim> RoleOperationClaims => Context.RoleOperationClaims;
    public DbSet<UserRoleClaim> UserRoleClaims => Context.UserRoleClaims;
    



    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await Context.SaveChangesAsync(cancellationToken);
    }

}
