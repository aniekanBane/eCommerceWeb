using eCommerceWeb.Domain.ValueObjects;
using FluentAssertions;

namespace eCommerceWeb.UnitTests.ValueObjects;

public class PhoneNumberTests
{
    [Theory]
    [InlineData("+1234567890")]
    [InlineData("1234567890")]
    [InlineData("+1 555 123 4567")]
    [InlineData("+44 7911 123456")]
    [InlineData("+91 98765-43210")]
    [InlineData("555-123-4567")]
    [InlineData("123 4567 8901")]
    [InlineData("+86 123 4567 8901")]
    public void Constructor_WithValidPhoneNumber_ShouldCreateInstance(string validNumber)
    {
        // Act
        var phoneNumber = new PhoneNumber(validNumber);

        // Assert
        phoneNumber.Number.Should().Be(validNumber.TrimStart('+').Replace(" ", "").Replace("-", ""));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_WithNullOrWhiteSpace_ShouldThrowException(string invalidNumber)
    {
        // Act & Assert
        FluentActions.Invoking(() => new PhoneNumber(invalidNumber))
            .Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("abc123")]
    [InlineData("++1234567890")]
    [InlineData("12")]
    [InlineData("12345678901234567890")]
    [InlineData("@#$%^&*")]
    [InlineData("+1.234.567.890")]
    [InlineData("(555) 123-4567")]
    public void Constructor_WithInvalidFormat_ShouldThrowException(string invalidNumber)
    {
        // Act & Assert
        FluentActions.Invoking(() => new PhoneNumber(invalidNumber))
            .Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("+1 234 567 890", "1234567890")]
    [InlineData("123-456-7890", "1234567890")]
    [InlineData("+44 7911-123-456", "447911123456")]
    public void Constructor_ShouldNormalizePhoneNumber(string input, string expected)
    {
        // Act
        var phoneNumber = new PhoneNumber(input);

        // Assert
        phoneNumber.Number.Should().Be(expected);
    }

    [Fact]
    public void Of_WithValidNumber_ShouldCreateInstance()
    {
        // Arrange
        var validNumber = "+1234567890";

        // Act
        var phoneNumber = PhoneNumber.Of(validNumber);

        // Assert
        phoneNumber.Number.Should().Be("1234567890");
    }

    [Fact]
    public void ExplicitOperator_WithValidNumber_ShouldCreateInstance()
    {
        // Arrange
        var validNumber = "+1234567890";

        // Act
        var phoneNumber = (PhoneNumber)validNumber;

        // Assert
        phoneNumber.Number.Should().Be("1234567890");
    }

    [Fact]
    public void ImplicitOperator_ToString_ShouldReturnNumber()
    {
        // Arrange
        var validNumber = "+1234567890";
        var phoneNumber = new PhoneNumber(validNumber);

        // Act
        string result = phoneNumber;

        // Assert
        result.Should().Be("1234567890");
    }

    [Fact]
    public void Equal_PhoneNumbers_ShouldBeEqual()
    {
        // Arrange
        var phone1 = new PhoneNumber("+1234567890");
        var phone2 = new PhoneNumber("+1234567890");

        // Act & Assert
        phone1.Should().Be(phone2);
    }

    [Fact]
    public void Different_PhoneNumbers_ShouldNotBeEqual()
    {
        // Arrange
        var phone1 = new PhoneNumber("+1234567890");
        var phone2 = new PhoneNumber("+9876543210");

        // Act & Assert
        phone1.Should().NotBe(phone2);
    }

    [Fact]
    public void Equal_PhoneNumbersWithDifferentFormat_ShouldBeEqual()
    {
        // Arrange
        var phone1 = new PhoneNumber("+1234567890");
        var phone2 = new PhoneNumber("1234567890");

        // Act & Assert
        phone1.Should().Be(phone2);
    }
} 