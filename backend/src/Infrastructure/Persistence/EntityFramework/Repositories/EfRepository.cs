using Domain.Entities;
using Infrastructure.Persistence.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityFramework.Repositories;

public sealed class EfRepository
{
    private BaseDbContext Context { get; }


    public EfRepository(BaseDbContext context)
    {
        Context = context;
    }


    public DbSet<User> Users => Context.Users;
    public DbSet<Streamer> Streamer => Context.Streamer;
    public DbSet<RefreshToken> RefreshToken => Context.RefreshToken;


    //public void Add<TEntity>(TEntity entity) where TEntity : Entity
    //{
    //    Context.Add(entity);
    //}

    //public async ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : Entity
    //{
    //    return await Context.AddAsync(entity, cancellationToken);
    //}

    //public void AddRange<TEntity>(IList<TEntity> entities) where TEntity : Entity
    //{
    //    Context.AddRange(entities);
    //}

    //public async Task AddRangeAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken) where TEntity : Entity
    //{
    //    await Context.AddRangeAsync(entities, cancellationToken);
    //}


    //public void Update<TEntity>(TEntity entity) where TEntity : Entity
    //{
    //    Context.Update(entity);
    //}

    //public void UpdateRange<TEntity>(IList<TEntity> entities) where TEntity : Entity
    //{
    //    Context.UpdateRange(entities);
    //}

    //public async Task<int> SaveChangesAsync()
    //{
    //    return await Context.SaveChangesAsync();
    //}

}
