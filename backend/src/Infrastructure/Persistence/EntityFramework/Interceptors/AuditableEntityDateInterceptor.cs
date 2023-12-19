using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel;

namespace Infrastructure.Persistence.EntityFramework.Interceptors;

public sealed class AuditableEntityDateInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            throw new ArgumentNullException(nameof(dbContext));
        }
        
        var auditableEntities = dbContext.ChangeTracker.Entries<AuditableEntity>();

        foreach (var data in auditableEntities)
        {
            _ = data.State switch
            {
                EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                EntityState.Modified => data.Entity.CreatedDate = DateTime.UtcNow,
                _ => DateTime.UtcNow
            };
        }
        
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}