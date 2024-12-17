using FluentAssertions;
using SharedKernel.Extensions;

namespace eCommerceWeb.UnitTests.Extensions;

public class EnumerableExtensionsTests
{
    [Fact]
    public void RandomChoice_WithSingleElement_ShouldReturnThatElement()
    {
        // Arrange
        var list = new[] { 1 };

        // Act
        var result = list.RandomChoice();

        // Assert
        result.Should().Be(1);
    }

    [Fact]
    public void RandomChoice_WithMultipleElements_ShouldReturnOneOfThem()
    {
        // Arrange
        var list = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = list.RandomChoice();

        // Assert
        list.Should().Contain(result);
    }

    [Theory]
    [InlineData(null)]
    public void RandomChoice_WithNullCollection_ShouldThrowException(int[]? invalidList)
    {
        // Act & Assert
        FluentActions.Invoking(() => invalidList!.RandomChoice())
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void RandomSubset_WithoutDuplicates_ShouldReturnUniqueElements()
    {
        // Arrange
        var list = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = list.RandomSubset(3).ToList();

        // Assert
        result.Should().HaveCount(3);
        result.Should().OnlyHaveUniqueItems();
        result.Should().AllSatisfy(item => list.Should().Contain(item));
    }

    [Fact]
    public void RandomSubset_WithDuplicates_ShouldAllowDuplicateElements()
    {
        // Arrange
        var list = new[] { 1, 2, 3 };

        // Act
        var result = list.RandomSubset(5, allowDuplicates: true).ToList();

        // Assert
        result.Should().HaveCount(5);
        result.Should().AllSatisfy(item => list.Should().Contain(item));
    }

    [Fact]
    public void Chunk_ShouldCreateEqualSizedChunks()
    {
        // Arrange
        var list = Enumerable.Range(1, 10);

        // Act
        var result = list.Chunk(3).ToList();

        // Assert
        result.Should().HaveCount(4);
        result.Take(3).Should().AllSatisfy(chunk => chunk.Should().HaveCount(3));
        result.Last().Should().HaveCount(1);
    }

    [Fact]
    public void IsNullOrEmpty_WithNullCollection_ShouldReturnTrue()
    {
        // Arrange
        List<int>? list = null;
        List<int> list2 = [];

        // Act & Assert
        list.IsNullOrEmpty().Should().BeTrue();
        list2.IsNullOrEmpty().Should().BeTrue();
    }
}