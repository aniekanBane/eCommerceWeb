namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed class ProductTag : Tag
{
    private ProductTag() : base() {}

    public ProductTag(string name) : base(name) { }
}
