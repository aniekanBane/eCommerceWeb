using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed class Category : AuditableEntity<int>, IAggregateRoot
{
    #pragma warning disable CS8618
    private Category() { } // EF Core

    public Category(CategoryCreationModel creationModel) 
    {
        SetName(creationModel.Name);
        Seo = new(creationModel.UrlSlug);
    }

    /* Details */
    public string Name { get; private set; }
    public string NormalisedName { get; private set; }

    /* Organization */
    public bool IsParentCategory 
    { 
        get => ParentCategoryId is null; 
        private set {} 
    }
    public int? ParentCategoryId { get; private set; }
    private readonly List<Category> _subCategories  = [];
    public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();

    /* Seo */
    public Seo Seo { get; private set; }
    public bool IsEnabled { get; private set; }
    public bool IsVisible { get; private set; }

    public Category AddSubCategory(Category category)
    {
        if (IsParentCategory || SubCategories.Count == 0)
            _subCategories.Add(category);
            
        return this;
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
    }
}
