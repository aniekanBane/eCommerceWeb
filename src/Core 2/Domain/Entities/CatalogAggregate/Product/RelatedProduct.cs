namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed class RelatedProduct(string product1Id, string product2Id)
{
    /// <summary> Product Id </summary>
    public string ProductId1 { get; private set; } = product1Id;
    
    /// <summary> Related Product Id </summary>
    public string ProductId2 { get; private set; } = product2Id;
}
