using eCommerceWeb.Domain.ValueObjects;
using FluentAssertions;

namespace eCommerceWeb.UnitTests.ValueObjects;

public class PercentTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public void Constructor_WithValidValue_ShouldCreateInstance(decimal value)
    {
        // Act
        var percent = new Percent(value);

        // Assert
        percent.Value.Should().Be(value);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-0.1)]
    public void Constructor_WithNegativeValue_ShouldThrowException(decimal negativeValue)
    {
        // Act & Assert
        FluentActions.Invoking(() => new Percent(negativeValue))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Zero_ShouldReturnZeroPercent()
    {
        // Act & Assert
        Percent.Zero.Value.Should().Be(0);
    }

    [Fact]
    public void Hundred_ShouldReturnHundredPercent()
    {
        // Act & Assert
        Percent.Hundered.Value.Should().Be(100);
    }

    [Fact]
    public void Of_WithValidValue_ShouldCreateInstance()
    {
        // Arrange
        var value = 75M;

        // Act
        var percent = Percent.Of(value);

        // Assert
        percent.Value.Should().Be(value);
    }

    [Theory]
    [InlineData(50, 25, true)]
    [InlineData(25, 50, false)]
    [InlineData(50, 50, false)]
    public void GreaterThan_Operator_ShouldCompareCorrectly(decimal left, decimal right, bool expected)
    {
        // Arrange
        var leftPercent = new Percent(left);
        var rightPercent = new Percent(right);

        // Act
        var result = leftPercent > rightPercent;

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(25, 50, true)]
    [InlineData(50, 25, false)]
    [InlineData(50, 50, false)]
    public void LessThan_Operator_ShouldCompareCorrectly(decimal left, decimal right, bool expected)
    {
        // Arrange
        var leftPercent = new Percent(left);
        var rightPercent = new Percent(right);

        // Act
        var result = leftPercent < rightPercent;

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(50, 25, true)]
    [InlineData(25, 50, false)]
    [InlineData(50, 50, true)]
    public void GreaterThanOrEqual_Operator_ShouldCompareCorrectly(decimal left, decimal right, bool expected)
    {
        // Arrange
        var leftPercent = new Percent(left);
        var rightPercent = new Percent(right);

        // Act
        var result = leftPercent >= rightPercent;

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(25, 50, true)]
    [InlineData(50, 25, false)]
    [InlineData(50, 50, true)]
    public void LessThanOrEqual_Operator_ShouldCompareCorrectly(decimal left, decimal right, bool expected)
    {
        // Arrange
        var leftPercent = new Percent(left);
        var rightPercent = new Percent(right);

        // Act
        var result = leftPercent <= rightPercent;

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Equal_Percents_ShouldBeEqual()
    {
        // Arrange
        var percent1 = new Percent(50);
        var percent2 = new Percent(50);

        // Act & Assert
        percent1.Should().Be(percent2);
    }

    [Fact]
    public void Different_Percents_ShouldNotBeEqual()
    {
        // Arrange
        var percent1 = new Percent(25);
        var percent2 = new Percent(75);

        // Act & Assert
        percent1.Should().NotBe(percent2);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(75.5)]
    public void Extensions_DecimalToPercent_ShouldCreateInstance(decimal value)
    {
        // Act
        var percent = value.Percent();

        // Assert
        percent.Value.Should().Be(value);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(75)]
    public void Extensions_IntToPercent_ShouldCreateInstance(int value)
    {
        // Act
        var percent = value.Percent();

        // Assert
        percent.Value.Should().Be(value);
    }
} 