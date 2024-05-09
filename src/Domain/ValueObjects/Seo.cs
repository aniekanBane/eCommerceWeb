namespace eCommerceWeb.Domain.ValueObjects;

public sealed record class Seo
{
    public string UrlSlug { get; init; }
    public string? MetaTitle { get; init; }
    public string? MetaKeywords { get; init; }
    public string? MetaDescription { get; init; }

    #pragma warning disable CS8618
    private Seo() { } // EF Core

    public Seo(string urlSlug) 
    {
        Guard.Against.NullOrWhiteSpace(urlSlug, nameof(urlSlug));
        Guard.Against.InvalidFormat(urlSlug, nameof(urlSlug), DomainModelConstants.SEO_URL_SLUG_REGEX);
        UrlSlug = urlSlug;
    }

    public Seo(
        string urlSlug, 
        string? metaTitle, 
        string? metaKeywords, 
        string? metaDescription) : this(urlSlug)
    {
        if (!string.IsNullOrWhiteSpace(metaTitle))
            Guard.Against.StringTooLong(metaTitle, DomainModelConstants.SEO_TITLE_MAX_LENGTH, nameof(metaTitle));
        if (!string.IsNullOrWhiteSpace(metaDescription))
            Guard.Against.StringTooLong(metaDescription, DomainModelConstants.SEO_DESC_MAX_LENGTH, nameof(metaDescription));
        if (!string.IsNullOrWhiteSpace(metaKeywords))
            Guard.Against.StringTooLong(metaKeywords, DomainModelConstants.SEO_IMAGE_DESC_MAX_LENGTH, nameof(metaKeywords));
            
        MetaTitle = metaTitle;
        MetaKeywords = metaKeywords;
        MetaDescription = metaDescription;
    }
}
