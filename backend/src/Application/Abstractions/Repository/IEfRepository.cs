﻿using Application.Contracts.Common.Models;
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
    DbSet<StreamOption> StreamOptions { get; }

    DbSet<StreamBlockedUser> StreamBlockedUsers { get; }
    DbSet<StreamFollowerUser> StreamFollowerUsers { get; }


    DbSet<OutboxMessage> OutboxMessages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Func<Task<List<GetStreamDto>>> GetLiveStreamersAsync(CancellationToken cancellationToken = default);
}