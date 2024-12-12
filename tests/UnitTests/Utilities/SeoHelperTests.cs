using FluentAssertions;
using SharedKernel.Constants;
using SharedKernel.Utilities;

namespace eCommerceWeb.UnitTests.Utilities;

public class SeoHelperTests
{
    [Theory]
    [InlineData("Hello World", "hello-world")]
    [InlineData("UPPER CASE", "upper-case")]
    [InlineData("special!@#$chars", "specialchars")]
    [InlineData("multiple   spaces", "multiple-spaces")]
    [InlineData("trailing-hyphen-", "trailing-hyphen")]
    [InlineData("-leading-hyphen", "leading-hyphen")]
    [InlineData("dots.and.underscores", "dotsandunderscores")]
    public void GenerateUrlSlug_WithVariousInputs_ShouldGenerateValidSlug(string input, string expected)
    {
        // Act
        var result = SeoHelper.GenerateUrlSlug(input);

        // Assert
        result.Should().Be(expected);
        result.Should().MatchRegex(DomainModelConstants.SEO_URL_SLUG_REGEX);
    }

    [Theory]
    [InlineData("Category/Subcategory", "category/subcategory")]
    [InlineData("/Leading/Slash", "leading/slash")]
    [InlineData("Trailing/Slash/", "trailing/slash")]
    [InlineData("Multiple///Slashes", "multiple/slashes")]
    [InlineData("Mixed/URL/with spaces", "mixed/url/with-spaces")]
    [InlineData("Category/Sub@#$Category", "category/subcategory")]
    public void GenerateUrlSlug_WithHierarchicalUrls_ShouldPreserveStructure(string input, string expected)
    {
        // Act
        var result = SeoHelper.GenerateUrlSlug(input);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void GenerateUrlSlug_WithComplexInput_ShouldGenerateCleanSlug()
    {
        // Arrange
        var input = "Product Category & Sub-Category @ Store!";
        var expected = "product-category-sub-category-store";

        // Act
        var result = SeoHelper.GenerateUrlSlug(input);

        // Assert
        result.Should().Be(expected);
    }
}