using Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Stream = Domain.Entities.Stream;

namespace Application.Abstractions.Repository;

public interface IEfRepository
{
    DbSet<User> Users { get; }
    
    DbSet<RefreshToken> RefreshTokens { get; }
    
    DbSet<OperationClaim> OperationClaims { get; }

    DbSet<UserRoleClaim> UserRoleClaims { get; }

    DbSet<RoleOperationClaim> RoleOperationClaims { get; }
    
    DbSet<UserOperationClaim> UserOperationClaims { get; }

    DbSet<Role> Roles { get; }

    DbSet<Stream> Streams { get; }
    DbSet<Streamer> Streamers { get; }

    DbSet<StreamChatMessage> StreamChatMessages { get; }



    DbSet<OutboxMessage> OutboxMessages { get; }
    
    DbSet<StreamModerator> StreamModerators { get; }
    


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}