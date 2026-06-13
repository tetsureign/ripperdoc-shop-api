using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RipperdocShop.Api.Models;
using RipperdocShop.Api.Models.Identities;

namespace RipperdocShop.Api.Interceptors;

public class TimestampInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateTimestamps(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateTimestamps(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateTimestamps(DbContext? context)
    {
        if (context == null) return;

        var now = DateTime.UtcNow;

        var entries = context.ChangeTracker
            .Entries<ITimestampedEntity>()
            .Where(e =>
            {
                var isAdded = e.State == EntityState.Added;
                var isModified = e.State == EntityState.Modified;

                var hasRealChanges = e.Properties
                    .Any(p => p.IsModified && p.Metadata.Name != nameof(ITimestampedEntity.UpdatedAt));

                return isAdded || (isModified && hasRealChanges);
            });

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = now;

            entry.Entity.UpdatedAt = now;
        }
    }
}
