using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.Primitives.SysTime;
using eCommerceWeb.Persistence.Extensions;
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
                    entry.Property(e => e.CreatedBy).CurrentValue = username;
                    entry.Property(e => e.CreatedOnUtc).CurrentValue = dateTime;
                    SetModifiedproperties(entry);
                    break;

                case EntityState.Modified:
                    SetModifiedproperties(entry);
                    break;

                case EntityState.Deleted when entry.Entity is ISoftDeleteEntity: 
                    entry.Property(nameof(ISoftDeleteEntity.IsDeleted)).CurrentValue = true;
                    entry.Property(nameof(ISoftDeleteEntity.DeletedBy)).CurrentValue = username;
                    entry.Property(nameof(ISoftDeleteEntity.DeletedOnUtc)).CurrentValue = dateTime;
                    entry.State = EntityState.Modified;
                    break; 

                case EntityState.Unchanged when entry.HasChangedOwnedEntities():
                    SetModifiedproperties(entry);
                    break;

                default: break;
            }
        }

        void SetModifiedproperties(EntityEntry<IAuditableEntity> entityEntry)
        {
            entityEntry.Property(e => e.LastModifiedBy).CurrentValue = username;
            entityEntry.Property(e => e.LastModifiedOnUtc).CurrentValue = dateTime;
        }
    }
}
