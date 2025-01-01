using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed class Category : AuditableEntity<int>, IAggregateRoot
{
    #pragma warning disable CS8618
    private Category() { } // EF Core
    #pragma warning restore CS8618

    public Category(CategoryCreationModel creationModel) 
    {
        SetName(creationModel.Name);
        Seo = new(creationModel.UrlSlug);
        IsEnabled = true;
        IsVisible = true;
    }

    /* Details */
    public string Name { get; private set; } = string.Empty;
    public string NormalizedName { get; private set; } = string.Empty;

    /* Organization */
    public bool IsParentCategory => ParentCategoryId is null; 
    public int? ParentCategoryId { get; private set; }
    private readonly List<Category> _subCategories  = [];
    public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();

    /* Seo */
    public Seo Seo { get; private set; }
    public bool IsEnabled { get; private set; }
    public bool IsVisible { get; private set; }

    public void AddSubCategory(Category category)
    {
        if (IsParentCategory || SubCategories.Count == 0)
            _subCategories.Add(category);
    }

    public Category Update(CategoryUpdateModel updateModel)
    {
        SetName(updateModel.Name);
        IsEnabled = updateModel.IsEnabled;
        IsVisible = updateModel.IsVisible;
        Seo = new(
            updateModel.UrlSlug, 
            updateModel.MetaTitle ,
            updateModel.MetaKeywords, 
            updateModel.MetaDescription
        );
        
        return this;
    }

    private void SetName(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.StringTooLong(name, DomainModelConstants.CATEGORY_NAME_MAX_LENGTH, nameof(name));

        Name = name;
        NormalizedName = name.Trim().ToUpperInvariant();
    }
}
