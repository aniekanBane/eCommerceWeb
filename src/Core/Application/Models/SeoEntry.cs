using FluentValidation;
using SharedKernel.Constants;

namespace eCommerceWeb.Application.Models;

public class SeoEntry
{
    public required string UrlSlug { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaKeywords { get; set; }
    public string? MetaDescription { get; set; }
}

internal sealed class SeoEntryValidator : AbstractValidator<SeoEntry>
{
    public SeoEntryValidator()
    {
        RuleFor(x => x.UrlSlug)
            .NotEmpty()
            .Matches(DomainModelConstants.SEO_URL_SLUG_REGEX);
        
        RuleFor(x => x.MetaTitle).MaximumLength(DomainModelConstants.SEO_TITLE_MAX_LENGTH);
        RuleFor(x => x.MetaKeywords).MaximumLength(DomainModelConstants.SEO_KEYWORDS_MAX_LENGTH);
        RuleFor(x => x.MetaDescription).MaximumLength(DomainModelConstants.SEO_DESC_MAX_LENGTH);
    }
}
