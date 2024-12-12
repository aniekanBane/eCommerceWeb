using eCommerceWeb.Domain.ValueObjects;
using FluentAssertions;

namespace eCommerceWeb.UnitTests.ValueObjects;

public class MoneyTests
{
    [Theory]
    [InlineData(100.126, 100.13)] // Rounds up
    [InlineData(100.124, 100.12)] // Rounds down
    [InlineData(100.125, 100.12)] // Rounds to even
    [InlineData(-100.126, -100.13)] // Negative rounds up
    [InlineData(-100.124, -100.12)] // Negative rounds down
    public void Constructor_ShouldRoundToTwoDecimals(decimal input, decimal expected)
    {
        // Act
        var money = new Money(input);

        // Assert
        money.Amount.Should().Be(expected);
    }

    [Fact]
    public void Zero_ShouldReturnZeroMoney()
    {
        // Act & Assert
        Money.Zero.Amount.Should().Be(0);
    }

    [Theory]
    [InlineData(100, 50, 150)]
    [InlineData(100.5, 50.5, 151)]
    [InlineData(-100, 50, -50)]
    public void Add_ShouldCorrectlyAddValues(decimal first, decimal second, decimal expected)
    {
        // Arrange
        var money1 = new Money(first);
        var money2 = new Money(second);

        // Act
        var result = money1.Add(money2);
        var resultFromDecimal = money1.Add(second);
        var resultFromOperator = money1 + money2;

        // Assert
        result.Amount.Should().Be(expected);
        resultFromDecimal.Amount.Should().Be(expected);
        resultFromOperator.Amount.Should().Be(expected);
    }

    [Theory]
    [InlineData(100, 50, 50)]
    [InlineData(100.5, 50.5, 50)]
    [InlineData(-100, 50, -150)]
    public void Subtract_ShouldCorrectlySubtractValues(decimal first, decimal second, decimal expected)
    {
        // Arrange
        var money1 = new Money(first);
        var money2 = new Money(second);

        // Act
        var result = money1.Subtract(money2);
        var resultFromDecimal = money1.Subtract(second);
        var resultFromOperator = money1 - money2;

        // Assert
        result.Amount.Should().Be(expected);
        resultFromDecimal.Amount.Should().Be(expected);
        resultFromOperator.Amount.Should().Be(expected);
    }

    [Theory]
    [InlineData(100, 2, 50)]
    [InlineData(100, 3, 33.33)]
    [InlineData(-100, 2, -50)]
    public void DivideBy_ShouldCorrectlyDivideValues(decimal amount, decimal divisor, decimal expected)
    {
        // Arrange
        var money = new Money(amount);
        var divisorMoney = new Money(divisor);

        // Act
        var result = money.DivideBy(divisorMoney);
        var resultFromDecimal = money.DivideBy(divisor);
        var resultFromOperator = money / divisorMoney;

        // Assert
        result.Amount.Should().Be(expected);
        resultFromDecimal.Amount.Should().Be(expected);
        resultFromOperator.Amount.Should().Be(expected);
    }

    [Theory]
    [InlineData(100, 2, 200)]
    [InlineData(100.5, 2, 201)]
    [InlineData(-100, 2, -200)]
    public void MultiplyBy_ShouldCorrectlyMultiplyValues(decimal amount, decimal multiplier, decimal expected)
    {
        // Arrange
        var money = new Money(amount);
        var multiplierMoney = new Money(multiplier);

        // Act
        var result = money.MultiplyBy(multiplierMoney);
        var resultFromDecimal = money.MultiplyBy(multiplier);
        var resultFromOperator = money * multiplierMoney;

        // Assert
        result.Amount.Should().Be(expected);
        resultFromDecimal.Amount.Should().Be(expected);
        resultFromOperator.Amount.Should().Be(expected);
    }

    [Theory]
    [InlineData(100, 10, 10)] // 10% of 100 is 10
    [InlineData(100, 25, 25)] // 25% of 100 is 25
    [InlineData(50, 50, 25)]  // 50% of 50 is 25
    [InlineData(200, 33.33, 66.66)] // 33.33% of 200 is 66.66
    [InlineData(-100, 10, -10)] // 10% of -100 is -10
    public void MultiplyByPercent_ShouldCorrectlyCalculatePercentage(decimal amount, decimal percentage, decimal expected)
    {
        // Arrange
        var money = new Money(amount);
        var percent = new Percent(percentage);

        // Act
        var result = money.MultiplyByPercent(percent);
        var resultFromDecimal = money.MultiplyByPercent(percentage);
        var resultFromOperator = money * percent;

        // Assert
        result.Amount.Should().Be(expected);
        resultFromDecimal.Amount.Should().Be(expected);
        resultFromOperator.Amount.Should().Be(expected);
    }

    [Theory]
    [InlineData(100, 0, 0)] // 0% of anything is 0
    [InlineData(100, 100, 100)] // 100% of amount is amount
    public void MultiplyByPercent_SpecialCases_ShouldCalculateCorrectly(decimal amount, decimal percentage, decimal expected)
    {
        // Arrange
        var money = new Money(amount);
        var percent = new Percent(percentage);

        // Act
        var result = money.MultiplyByPercent(percent);

        // Assert
        result.Amount.Should().Be(expected);
    }

    [Theory]
    [InlineData(100, 50, true)]
    [InlineData(50, 100, false)]
    [InlineData(100, 100, false)]
    public void GreaterThan_Operator_ShouldCompareCorrectly(decimal left, decimal right, bool expected)
    {
        // Arrange
        var leftMoney = new Money(left);
        var rightMoney = new Money(right);

        // Act
        var result = leftMoney > rightMoney;

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(50, 100, true)]
    [InlineData(100, 50, false)]
    [InlineData(100, 100, false)]
    public void LessThan_Operator_ShouldCompareCorrectly(decimal left, decimal right, bool expected)
    {
        // Arrange
        var leftMoney = new Money(left);
        var rightMoney = new Money(right);

        // Act
        var result = leftMoney < rightMoney;

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(100, 50, true)]
    [InlineData(50, 100, false)]
    [InlineData(100, 100, true)]
    public void GreaterThanOrEqual_Operator_ShouldCompareCorrectly(decimal left, decimal right, bool expected)
    {
        // Arrange
        var leftMoney = new Money(left);
        var rightMoney = new Money(right);

        // Act
        var result = leftMoney >= rightMoney;

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(50, 100, true)]
    [InlineData(100, 50, false)]
    [InlineData(100, 100, true)]
    public void LessThanOrEqual_Operator_ShouldCompareCorrectly(decimal left, decimal right, bool expected)
    {
        // Arrange
        var leftMoney = new Money(left);
        var rightMoney = new Money(right);

        // Act
        var result = leftMoney <= rightMoney;

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Equal_Money_ShouldBeEqual()
    {
        // Arrange
        var money1 = new Money(100.00M);
        var money2 = new Money(100.00M);

        // Act & Assert
        money1.Should().Be(money2);
    }

    [Fact]
    public void Different_Money_ShouldNotBeEqual()
    {
        // Arrange
        var money1 = new Money(100.00M);
        var money2 = new Money(200.00M);

        // Act & Assert
        money1.Should().NotBe(money2);
    }

    [Fact]
    public void Of_ShouldCreateNewInstance()
    {
        // Arrange
        var amount = 123.45M;

        // Act
        var money = Money.Of(amount);

        // Assert
        money.Amount.Should().Be(amount);
    }
} 