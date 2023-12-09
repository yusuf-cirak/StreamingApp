using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Repository;

public interface IEfRepository
{
    public DbSet<User> Users { get; set; }
    public DbSet<Streamer> Streamers { get; set; }

    public DbSet<RefreshToken> RefreshToken { get; set; }

}
