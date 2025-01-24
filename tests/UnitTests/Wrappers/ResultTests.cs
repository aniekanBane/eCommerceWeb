using FluentAssertions;
using SharedKernel.Wrappers;

namespace eCommerceWeb.UnitTests.Wrappers;

public class ResultTests
{
    #region Base Result Tests
    [Fact]
    public void Success_ShouldCreateSuccessResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
        result.Message.Should().BeNull();
    }

    [Fact]
    public void Failure_WithError_ShouldCreateFailureResult()
    {
        // Arrange
        var error = Error.NotFound("Resource not found");

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void ImplicitConversion_FromError_ShouldCreateFailureResult()
    {
        // Arrange
        Error error = Error.Invalid("Invalid input");

        // Act
        Result result = error;

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }
    #endregion

    #region Generic Result Tests
    [Fact]
    public void Success_WithData_ShouldCreateSuccessResult()
    {
        // Arrange
        var data = "Test Data";

        // Act
        var result = Result.Success<string>(data);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(data);
        result.Error.Should().BeNull();
    }

    [Fact]
    public void ImplicitConversion_ToData_ShouldReturnData()
    {
        // Arrange
        var result = Result.Success<string>("Test Data");

        // Act
        string data = result;

        // Assert
        data.Should().Be("Test Data");
    }

    [Fact]
    public void ImplicitConversion_FromData_ShouldCreateSuccessResult()
    {
        // Arrange
        string data = "Test Data";

        // Act
        Result<string> result = data;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(data);
    }

    [Fact]
    public void ImplicitConversion_T_FromError_ShouldCreateFailureResult()
    {
        // Arrange
        Error error = Error.UnAvailible();

        // Act
        Result<string> result = error;

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
        FluentActions.Invoking(() => result.Data)
            .Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AccessingData_OnFailure_ShouldThrowException()
    {
        // Arrange
        var result = Result.Failure<string>(Error.Invalid("Invalid"));

        // Act & Assert
        FluentActions.Invoking(() => result.Data)
            .Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot access failure result data.");
    }
    #endregion

    #region FirstFailureOrSuccess Tests
    [Fact]
    public void FirstFailureOrSuccess_WithNoFailures_ShouldReturnSuccess()
    {
        // Arrange
        var results = new[]
        {
            Result.Success("First"),
            Result.Success("Second"),
            Result.Success("Third")
        };

        // Act
        var result = Result.FirstFailureOrSuccess(results);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public void FirstFailureOrSuccess_WithFailures_ShouldReturnFirstFailure()
    {
        // Arrange
        var error1 = Error.NotFound("First error");
        var error2 = Error.Invalid("Second error");
        var results = new[]
        {
            Result.Success("Success"),
            Result.Failure(error1),
            Result.Failure(error2)
        };

        // Act
        var result = Result.FirstFailureOrSuccess(results);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error1);
    }

    [Fact]
    public void FirstFailureOrSuccess_WithEmptyArray_ShouldReturnSuccess()
    {
        // Act
        var result = Result.FirstFailureOrSuccess();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
    }
    #endregion
}
