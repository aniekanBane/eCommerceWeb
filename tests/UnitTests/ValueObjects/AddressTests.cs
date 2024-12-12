using eCommerceWeb.Domain.ValueObjects;
using FluentAssertions;
using SharedKernel.Constants;

namespace eCommerceWeb.UnitTests.ValueObjects;

public class AddressTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateInstance()
    {
        // Arrange
        var line1 = "123 Main St";
        var line2 = "Apt 4B";
        var city = "New York";
        var stateProvince = "NY";
        var zipCode = "10001";
        var country = "USA";

        // Act
        var address = new Address(line1, line2, city, stateProvince, zipCode, country);

        // Assert
        address.Line1.Should().Be(line1);
        address.Line2.Should().Be(line2);
        address.City.Should().Be(city);
        address.StateProvince.Should().Be(stateProvince);
        address.ZipCode.Should().Be(zipCode);
        address.Country.Should().Be(country);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_WithNullOrWhiteSpaceRequiredFields_ShouldThrowException(string invalidValue)
    {
        // Arrange
        var validLine1 = "123 Main St";
        var validCity = "New York";
        var validStateProvince = "NY";
        var validCountry = "USA";

        // Act & Assert
        FluentActions.Invoking(() => new Address(invalidValue, null, validCity, validStateProvince, null, validCountry))
            .Should().Throw<ArgumentException>(); // Line1

        FluentActions.Invoking(() => new Address(validLine1, null, invalidValue, validStateProvince, null, validCountry))
            .Should().Throw<ArgumentException>(); // City

        FluentActions.Invoking(() => new Address(validLine1, null, validCity, invalidValue, null, validCountry))
            .Should().Throw<ArgumentException>(); // StateProvince

        FluentActions.Invoking(() => new Address(validLine1, null, validCity, validStateProvince, null, invalidValue))
            .Should().Throw<ArgumentException>(); // Country
    }

    [Theory]
    [MemberData(nameof(GetTooLongFieldValues))]
    public void Constructor_WithTooLongFields_ShouldThrowException(
        string line1,
        string? line2,
        string city,
        string stateProvince,
        string? zipCode,
        string country)
    {
        // Act & Assert
        FluentActions.Invoking(() => 
            new Address(line1, line2, city, stateProvince, zipCode, country))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WithNullOptionalFields_ShouldCreateInstance()
    {
        // Arrange
        var line1 = "123 Main St";
        var city = "New York";
        var stateProvince = "NY";
        var country = "USA";

        // Act
        var address = new Address(line1, null, city, stateProvince, null, country);

        // Assert
        address.Line2.Should().BeNull();
        address.ZipCode.Should().BeNull();
    }

    [Fact]
    public void Equal_Addresses_ShouldBeEqual()
    {
        // Arrange
        var address1 = new Address(
            "123 Main St",
            "Apt 4B",
            "New York",
            "NY",
            "10001",
            "USA"
        );

        var address2 = new Address(
            "123 Main St",
            "Apt 4B",
            "New York",
            "NY",
            "10001",
            "USA"
        );

        // Act & Assert
        address1.Should().Be(address2);
    }

    [Fact]
    public void Different_Addresses_ShouldNotBeEqual()
    {
        // Arrange
        var address1 = new Address(
            "123 Main St",
            "Apt 4B",
            "New York",
            "NY",
            "10001",
            "USA"
        );

        var address2 = new Address(
            "456 Oak Ave",
            "Suite 2C",
            "Boston",
            "MA",
            "02108",
            "USA"
        );

        // Act & Assert
        address1.Should().NotBe(address2);
    }

    [Fact]
    public void Equal_AddressesWithNullOptionalFields_ShouldBeEqual()
    {
        // Arrange
        var address1 = new Address(
            "123 Main St",
            null,
            "New York",
            "NY",
            null,
            "USA"
        );

        var address2 = new Address(
            "123 Main St",
            null,
            "New York",
            "NY",
            null,
            "USA"
        );

        // Act & Assert
        address1.Should().Be(address2);
    }

    public static IEnumerable<object?[]> GetTooLongFieldValues()
    {
        // Too long Line1
        yield return new object?[] 
        { 
            new string('x', DomainModelConstants.ADDRESS_LINE_MAX_LENGTH + 1),
            null,
            "New York",
            "NY",
            null,
            "USA"
        };

        // Too long Line2
        yield return new object?[] 
        { 
            "123 Main St",
            new string('x', DomainModelConstants.ADDRESS_LINE_MAX_LENGTH + 1),
            "New York",
            "NY",
            null,
            "USA"
        };

        // Too long City
        yield return new object?[] 
        { 
            "123 Main St",
            null,
            new string('x', DomainModelConstants.CITY_MAX_LENGTH + 1),
            "NY",
            null,
            "USA"
        };

        // Too long ZipCode
        yield return new object?[] 
        { 
            "123 Main St",
            null,
            "New York",
            "NY",
            new string('x', DomainModelConstants.ZIPCODE_MAX_LENGTH + 1),
            "USA"
        };

        // Too long Country
        yield return new object?[] 
        { 
            "123 Main St",
            null,
            "New York",
            "NY",
            null,
            new string('x', DomainModelConstants.COUNTRY_NAME_MAX_LENGTH + 1)
        };
    }
} 