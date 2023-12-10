using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Repository;

public interface IEfRepository
{
    DbSet<User> Users { get; }
    DbSet<Streamer> Streamers { get; }
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}
