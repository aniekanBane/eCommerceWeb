using FluentAssertions;
using SharedKernel.Wrappers;

namespace eCommerceWeb.UnitTests.Wrappers;

public class ResultExtensionsTests
{
    #region Bind Tests
    [Fact]
    public void Bind_WithSuccess_ShouldTransformResult()
    {
        // Arrange
        var result = Result.Success(5);
        static Result<string> Transform(int number) => Result.Success<string>($"Number: {number}");

        // Act
        var boundResult = result.Bind(Transform);

        // Assert
        boundResult.IsSuccess.Should().BeTrue();
        boundResult.Data.Should().Be("Number: 5");
    }

    [Fact]
    public void Bind_WithFailure_ShouldPropagateError()
    {
        // Arrange
        var error = Error.Invalid("Invalid input");
        var result = Result.Failure<int>(error);
        static Result<string> Transform(int number) => Result.Success<string>($"Number: {number}");

        // Act
        var boundResult = result.Bind(Transform);

        // Assert
        boundResult.IsSuccess.Should().BeFalse();
        boundResult.Error.Should().Be(error);
    }

    [Fact]
    public void Bind_FromBaseResult_ShouldTransformToGeneric()
    {
        // Arrange
        var result = Result.Success();
        static Result<int> Transform(Result _) => Result.Success(42);

        // Act
        var boundResult = result.Bind(Transform);

        // Assert
        boundResult.IsSuccess.Should().BeTrue();
        boundResult.Data.Should().Be(42);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(42)]
    public void Bind_FromGenericToBase_ShouldTransform(int value)
    {
        // Arrange
        var result = Result.Success(value);
        static Result Transform(int number) => number > 0 
            ? Result.Success() 
            : Result.Failure(Error.Invalid("Must be positive"));

        // Act
        var boundResult = result.Bind(Transform);

        // Assert
        if (value > 0) 
            boundResult.IsSuccess.Should().BeTrue();
        else
            boundResult.IsSuccess.Should().BeFalse();
    }
    #endregion

    #region Ensure Tests
    [Fact]
    public void Ensure_WhenPredicateTrue_ShouldReturnOriginalResult()
    {
        // Arrange
        var result = Result.Success(10);

        // Act
        var ensured = result.Ensure(x => x > 0, Error.Invalid("Must be positive"));

        // Assert
        ensured.IsSuccess.Should().BeTrue();
        ensured.Data.Should().Be(10);
    }

    [Fact]
    public void Ensure_WhenPredicateFalse_ShouldReturnFailure()
    {
        // Arrange
        var result = Result.Success(0);
        var error = Error.Invalid("Must be positive");

        // Act
        var ensured = result.Ensure(x => x > 0, error);

        // Assert
        ensured.IsSuccess.Should().BeFalse();
        ensured.Error.Should().Be(error);
    }

    [Fact]
    public void Ensure_WithFailureResult_ShouldPropagateError()
    {
        // Arrange
        var originalError = Error.NotFound("Not found");
        var result = Result.Failure<int>(originalError);
        var ensureError = Error.Invalid("Must be positive");

        // Act
        var ensured = result.Ensure(x => x > 0, ensureError);

        // Assert
        ensured.IsSuccess.Should().BeFalse();
        ensured.Error.Should().Be(originalError);
    }
    #endregion

    #region Map Tests
    [Fact]
    public void Map_WithSuccess_ShouldTransformData()
    {
        // Arrange
        var result = Result.Success(5);

        // Act
        var mapped = result.Map(x => x.ToString());

        // Assert
        mapped.IsSuccess.Should().BeTrue();
        mapped.Data.Should().Be("5");
    }

    [Fact]
    public void Map_WithFailure_ShouldPropagateError()
    {
        // Arrange
        var error = Error.NotFound("Not found");
        var result = Result.Failure<int>(error);

        // Act
        var mapped = result.Map(x => x.ToString());

        // Assert
        mapped.IsSuccess.Should().BeFalse();
        mapped.Error.Should().Be(error);
    }

    [Fact]
    public void Map_FromBaseResult_ShouldTransform()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var mapped = result.Map(() => 42);

        // Assert
        mapped.IsSuccess.Should().BeTrue();
        mapped.Data.Should().Be(42);
    }
    #endregion

    #region Match Tests
    [Fact]
    public void Match_WithSuccess_ShouldExecuteSuccessFunc()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var matched = result.Match(
            onSuccess: () => "Success",
            onFailure: error => "Failure"
        );

        // Assert
        matched.Should().Be("Success");
    }

    [Fact]
    public void Match_WithFailure_ShouldExecuteFailureFunc()
    {
        // Arrange
        var error = Error.NotFound("Not found");
        var result = Result.Failure(error);

        // Act
        var matched = result.Match(
            onSuccess: () => "Success",
            onFailure: err => $"Failure: {err.Message}"
        );

        // Assert
        matched.Should().Be("Failure: Not found");
    }
    #endregion

    #region PagedResult Tests
    [Fact]
    public void ToPagedResult_ShouldReturnPagedResult_WhenResultIsSuccess()
    {
        // Arrange
        List<string> data = [ "item1", "item2", "item3" ];
        var result = Result.Success(data);
        var pagination = new Pagination(1, 10, data.Count);

        // Act
        var pagedResult = result.Map(x => x.AsEnumerable()).ToPagedResult(pagination);

        // Assert
        pagedResult.Should().NotBeNull();
        pagedResult.Data.Should().BeEquivalentTo(data);
        pagedResult.Pagination.Should().Be(pagination);
    }
    #endregion
}
