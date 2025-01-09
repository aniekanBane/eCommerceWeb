using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace eCommerceWeb.Persistence.Extensions;

internal static class AuditableExtensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r 
            => r.TargetEntry is not null 
            && r.TargetEntry.Metadata.IsOwned() 
            && r.TargetEntry.State is EntityState.Added or EntityState.Modified
        );
    }
}
