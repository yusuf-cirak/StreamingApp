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

    public DbSet<User> Users { get => Context.Users; }
    public DbSet<Streamer> Streamers { get => Context.Streamers; }
    public DbSet<RefreshToken> RefreshTokens { get => Context.RefreshTokens; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await Context.SaveChangesAsync(cancellationToken);
    }

}
