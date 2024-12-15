namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed class RelatedProduct
{
    public RelatedProduct(string product1Id, string product2Id)
    {
        Product1Id = product1Id;
        Product2Id = product2Id;
    }

    #pragma warning disable CS8618
    private RelatedProduct() { } // EF Core
    #pragma warning restore CS8618

    /// <summary> Product Id </summary>
    public string Product1Id { get; private set; }
    
    /// <summary> Related Product Id </summary>
    public string Product2Id { get; private set; }
}
