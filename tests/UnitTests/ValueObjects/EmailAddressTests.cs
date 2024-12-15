using eCommerceWeb.Domain.ValueObjects;
using FluentAssertions;

namespace eCommerceWeb.UnitTests.ValueObjects;

public class EmailAddressTests
{
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name+tag@example.co.uk")]
    [InlineData("user.name@subdomain.example.com")]
    public void Constructor_WithValidEmail_ShouldCreateInstance(string validEmail)
    {
        // Act
        var emailAddress = new EmailAddress(validEmail);

        // Assert
        emailAddress.Value.Should().Be(validEmail);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-email")]
    [InlineData("invalid@")]
    [InlineData("@invalid.com")]
    [InlineData("invalid@.com")]
    public void Constructor_WithInvalidEmail_ShouldThrowException(string invalidEmail)
    {
        // Act & Assert
        FluentActions.Invoking(() => new EmailAddress(invalidEmail)).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Of_WithValidEmail_ShouldCreateInstance()
    {
        // Arrange
        var validEmail = "test@example.com";

        // Act
        var emailAddress = EmailAddress.Of(validEmail);

        // Assert
        emailAddress.Value.Should().Be(validEmail);
    }

    [Fact]
    public void ExplicitOperator_WithValidEmail_ShouldCreateInstance()
    {
        // Arrange
        var validEmail = "test@example.com";

        // Act
        var emailAddress = (EmailAddress)validEmail;

        // Assert
        emailAddress.Value.Should().Be(validEmail);
    }

    [Fact]
    public void ImplicitOperator_ToString_ShouldReturnValue()
    {
        // Arrange
        var validEmail = "test@example.com";
        var emailAddress = new EmailAddress(validEmail);

        // Act
        string result = emailAddress;

        // Assert
        result.Should().Be(validEmail);
    }

    [Fact]
    public void Equal_EmailAddresses_ShouldBeEqual()
    {
        // Arrange
        var email1 = new EmailAddress("test@example.com");
        var email2 = new EmailAddress("test@example.com");

        // Act & Assert
        email1.Should().Be(email2);
    }

    [Fact]
    public void Different_EmailAddresses_ShouldNotBeEqual()
    {
        // Arrange
        var email1 = new EmailAddress("test1@example.com");
        var email2 = new EmailAddress("test2@example.com");

        // Act & Assert
        email1.Should().NotBe(email2);
    }
}
