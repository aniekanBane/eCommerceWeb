using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.Primitives.SysTime;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace eCommerceWeb.Persistence.Interceptors;

internal sealed class AuditableEntitySaveInterceptor(IDateTimeProvider dateTimeProvider) // TODO: Inject CurrentUserService
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, 
        InterceptionResult<int> result)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAudit(DbContext? dbContext)
    {
        if (dbContext is null) return;

        var username = "Initiator"; // TODO: fetch from UserService
        var dateTime = dateTimeProvider.UtcNow;

        foreach (var entry in dbContext.ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch(entry.State)
            {
                case EntityState.Added:
                    entry.Property(e => e.Author).CurrentValue = username;
                    entry.Property(e => e.CreatedOnUtc).CurrentValue = dateTime;
                    entry.Property(e => e.Editor).CurrentValue = username;
                    entry.Property(e => e.LastModifiedOnUtc).CurrentValue = dateTime;
                    break;

                case EntityState.Modified:
                    entry.Property(e => e.Editor).CurrentValue = username;
                    entry.Property(e => e.LastModifiedOnUtc).CurrentValue = dateTime;
                    break;

                case EntityState.Deleted when entry.Entity is ISoftDeleteEntity: 
                    entry.Property("IsDeleted").CurrentValue = true;
                    entry.State = EntityState.Modified;
                    break; 

                case EntityState.Unchanged when entry.HasChangedOwnedEntities():
                    entry.Property(e => e.Editor).CurrentValue = username;
                    entry.Property(e => e.LastModifiedOnUtc).CurrentValue = dateTime;
                    break;

                default: break;
            }
        }
    }
}

internal static class AuditEntityEntryExtensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r 
            => r.TargetEntry is not null 
            && r.TargetEntry.Metadata.IsOwned() 
            && r.TargetEntry.State is EntityState.Added or EntityState.Modified);
    }
}
