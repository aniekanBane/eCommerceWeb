using eCommerceWeb.Domain.ValueObjects;
using FluentAssertions;
using SharedKernel.Constants;
using SharedKernel.Utilities;

namespace eCommerceWeb.UnitTests.ValueObjects;

public class SeoTests
{
    [Theory]
    [InlineData("product-name")]
    [InlineData("category/subcategory")]
    [InlineData("blog-post-123")]
    [InlineData("a-very-long-url-slug-with-numbers-123")]
    public void Constructor_WithValidUrlSlug_ShouldCreateInstance(string validSlug)
    {
        // Act
        var seo = new Seo(validSlug);

        // Assert
        seo.UrlSlug.Should().Be(validSlug);
        seo.MetaTitle.Should().BeNull();
        seo.MetaKeywords.Should().BeNull();
        seo.MetaDescription.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_WithNullOrWhiteSpaceUrlSlug_ShouldThrowException(string invalidSlug)
    {
        // Act & Assert
        FluentActions.Invoking(() => new Seo(invalidSlug))
            .Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("Product Name")] // Contains space
    [InlineData("product_name")] // Contains underscore
    [InlineData("UPPERCASE")] // Contains uppercase
    [InlineData("product@name")] // Contains special character
    [InlineData("product.name")] // Contains dot
    public void Constructor_WithInvalidUrlSlugFormat_ShouldThrowException(string invalidSlug)
    {
        // Act & Assert
        FluentActions.Invoking(() => new Seo(invalidSlug))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WithValidMetadata_ShouldCreateInstance()
    {
        // Arrange
        var urlSlug = "test-product";
        var metaTitle = "Test Product";
        var metaKeywords = "test, product, keywords";
        var metaDescription = "This is a test product description";

        // Act
        var seo = new Seo(urlSlug, metaTitle, metaKeywords, metaDescription);

        // Assert
        seo.UrlSlug.Should().Be(urlSlug);
        seo.MetaTitle.Should().Be(metaTitle);
        seo.MetaKeywords.Should().Be(metaKeywords);
        seo.MetaDescription.Should().Be(metaDescription);
    }

    [Theory]
    [MemberData(nameof(GetInvalidMetadataLengths))]
    public void Constructor_WithTooLongMetadata_ShouldThrowException(
        string metaTitle, 
        string metaKeywords, 
        string metaDescription)
    {
        // Arrange
        var urlSlug = "test-product";

        // Act & Assert
        FluentActions.Invoking(() => new Seo(urlSlug, metaTitle, metaKeywords, metaDescription))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equal_Seos_ShouldBeEqual()
    {
        // Arrange
        var seo1 = new Seo(
            "test-product",
            "Test Product",
            "test, product",
            "Test description"
        );

        var seo2 = new Seo(
            "test-product",
            "Test Product",
            "test, product",
            "Test description"
        );

        // Act & Assert
        seo1.Should().Be(seo2);
    }

    [Fact]
    public void Different_Seos_ShouldNotBeEqual()
    {
        // Arrange
        var seo1 = new Seo(
            "test-product-1",
            "Test Product 1",
            "test, product",
            "Test description"
        );

        var seo2 = new Seo(
            "test-product-2",
            "Test Product 2",
            "test, product",
            "Test description"
        );

        // Act & Assert
        seo1.Should().NotBe(seo2);
    }

    [Theory]
    [InlineData("Product Name", "product-name")]
    [InlineData("UPPERCASE TEXT", "uppercase-text")]
    [InlineData("Special@#$% Characters", "special-characters")]
    [InlineData("Multiple     Spaces", "multiple-spaces")]
    [InlineData("Product.With.Dots", "productwithdots")]
    [InlineData("Product_With_Underscore", "productwithunderscore")]
    [InlineData("   Trim Spaces   ", "trim-spaces")]
    public void Constructor_WithGeneratedUrlSlug_ShouldCreateValidInstance(string input, string expectedSlug)
    {
        // Arrange
        var urlSlug = SeoHelper.GenerateUrlSlug(input);

        // Act
        var seo = new Seo(urlSlug);

        // Assert
        seo.UrlSlug.Should().Be(expectedSlug);
    }

    [Theory]
    [InlineData("Product & Category", "product-category")]
    [InlineData("Electronics & Gadgets", "electronics-gadgets")]
    [InlineData("Phones & Tablets", "phones-tablets")]
    public void Constructor_WithAmpersandInInput_ShouldCreateValidUrlSlug(string input, string expectedSlug)
    {
        // Arrange
        var urlSlug = SeoHelper.GenerateUrlSlug(input);

        // Act
        var seo = new Seo(urlSlug);

        // Assert
        seo.UrlSlug.Should().Be(expectedSlug);
    }

    [Theory]
    [InlineData("Product/Category", "product/category")]
    [InlineData("Electronics/Phones/Samsung", "electronics/phones/samsung")]
    public void Constructor_WithForwardSlashInInput_ShouldPreserveSlashes(string input, string expectedSlug)
    {
        // Arrange
        var urlSlug = SeoHelper.GenerateUrlSlug(input);

        // Act
        var seo = new Seo(urlSlug);

        // Assert
        seo.UrlSlug.Should().Be(expectedSlug);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("@#$%^&*")]
    public void GenerateUrlSlug_WithInvalidInput_ShouldReturnEmptyString(string invalidInput)
    {
        // Act
        var result = SeoHelper.GenerateUrlSlug(invalidInput);

        // Assert
        result.Should().BeEmpty();
        FluentActions.Invoking(() => new Seo(result))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GenerateUrlSlug_WithLongInput_ShouldCreateValidSlug()
    {
        // Arrange
        var longInput = "This is a very long product name with special characters @#$% and UPPERCASE letters";
        var expectedSlug = "this-is-a-very-long-product-name-with-special-characters-and-uppercase-letters";

        // Act
        var urlSlug = SeoHelper.GenerateUrlSlug(longInput);
        var seo = new Seo(urlSlug);

        // Assert
        seo.UrlSlug.Should().Be(expectedSlug);
    }

    public static IEnumerable<object[]> GetInvalidMetadataLengths()
    {
        // Too long meta title
        yield return new object[] 
        { 
            new string('x', DomainModelConstants.SEO_TITLE_MAX_LENGTH + 1),
            "keywords",
            "description"
        };

        // Too long meta keywords
        yield return new object[] 
        { 
            "title",
            new string('x', DomainModelConstants.SEO_KEYWORDS_MAX_LENGTH + 1),
            "description"
        };

        // Too long meta description
        yield return new object[] 
        { 
            "title",
            "keywords",
            new string('x', DomainModelConstants.SEO_DESC_MAX_LENGTH + 1)
        };
    }
} 