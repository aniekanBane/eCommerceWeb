using Ardalis.Specification;
using eCommerceWeb.Domain.Entities.CatalogAggregate;

namespace eCommerceWeb.Domain.Specifications;

public class ProductItemsSpec : Specification<Product>
{
    public ProductItemsSpec(params string[] ids)
    {
        Query.Where(p => ids.Contains(p.Id));
    }
}

public class ProductNameSpec : Specification<Product>
{
    public ProductNameSpec(string name)
    {
        Query.Where(p => p.NormalizedName == name.ToUpper());
    }
}

public class ProductSkuSpec : Specification<Product>
{
    public ProductSkuSpec(string sku)
    {
        Query.Where(p => p.NormalizedSku == sku.ToUpper());        
    }
}

public class ProductFilterSpec : Specification<Product>
{
    public ProductFilterSpec(string? filter)
    {
        if (!string.IsNullOrWhiteSpace(filter))
        {
            filter = filter.ToUpper();
            Query.AsNoTracking().Include(p => p.Categories)
                .Where(p => p.NormalizedName.Contains(filter)
                || p.Tags.Any(t => t.NormalisedName.Contains(filter))
                || p.Categories.Any(c => c.NormalizedName.Contains(filter)))
                .OrderBy(p => p.Name);
        }
    }
}

public class ProductPagedFilterSpec : ProductFilterSpec
{
    public ProductPagedFilterSpec(string? filter, int pageNumber, int pageSize) 
        : base(filter)
    {
        Query.AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        if (string.IsNullOrWhiteSpace(filter))
            Query.OrderByDescending(p => p.LastModifiedOnUtc);

        Query.AsSplitQuery();
    }
}
