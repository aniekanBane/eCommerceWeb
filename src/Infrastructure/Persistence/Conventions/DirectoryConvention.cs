using eCommerceWeb.Domain.Entities.Directory;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace eCommerceWeb.Persistence.Conventions;

internal sealed class DirectoryConvention : IModelFinalizingConvention
{
    public void ProcessModelFinalizing(
        IConventionModelBuilder modelBuilder, 
        IConventionContext<IConventionModelBuilder> context)
    {
        foreach (var entity in modelBuilder.Metadata.GetEntityTypes()
            .Where(e => e.ClrType.IsSubclassOf(typeof(DirectoryBase))))
        {
            var normalizedNameProperty = entity.FindProperty(nameof(DirectoryBase.NormalizedName));
            var nameProperty = entity.FindProperty(nameof(DirectoryBase.Name));
            
            nameProperty?.SetColumnOrder(1);
            normalizedNameProperty?.SetColumnOrder(2);
        }
    }
}
