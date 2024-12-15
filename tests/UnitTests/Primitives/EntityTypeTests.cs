using eCommerceWeb.Domain.Primitives.Entities;
using FluentAssertions;

namespace eCommerceWeb.UnitTests.Primitives;

public class EntityTypeTests
{
    [Fact]
    public void Equals_WithSameId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act & Assert
        entity1.Should().Be(entity2);
        entity1.Equals(entity2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentId_ShouldReturnFalse()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act & Assert
        entity1.Should().NotBe(entity2);
        entity1.Equals(entity2).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());

        // Act & Assert
        entity.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = new TestEntity(id);
        var differentEntity = new DifferentTestEntity(id);

        // Act & Assert
        entity.Equals(differentEntity).Should().BeFalse();
    }

    [Fact]
    public void EqualityOperator_WithSameReference_ShouldReturnTrue()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());
        var entityRef = entity;

        // Act & Assert
        (entity == entityRef).Should().BeTrue();
    }

    [Fact]
    public void EqualityOperator_WithBothNull_ShouldReturnTrue()
    {
        // Arrange
        TestEntity? entity1 = null;
        TestEntity? entity2 = null;

        // Act & Assert
        (entity1 == entity2).Should().BeTrue();
    }

    [Fact]
    public void EqualityOperator_WithOneNull_ShouldReturnFalse()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());
        TestEntity? nullEntity = null;

        // Act & Assert
        (entity == nullEntity).Should().BeFalse();
        (nullEntity == entity).Should().BeFalse();
    }

    [Fact]
    public void InequalityOperator_ShouldReturnOppositeOfEqualityOperator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);
        var entity3 = new TestEntity(Guid.NewGuid());

        // Act & Assert
        (entity1 != entity2).Should().BeFalse();
        (entity1 != entity3).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_WithSameId_ShouldReturnSameValue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act
        var hashCode1 = entity1.GetHashCode();
        var hashCode2 = entity2.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentIds_ShouldReturnDifferentValues()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act
        var hashCode1 = entity1.GetHashCode();
        var hashCode2 = entity2.GetHashCode();

        // Assert
        hashCode1.Should().NotBe(hashCode2);
    }

    private class TestEntity(Guid id) : Entity<Guid>(id);

    private class DifferentTestEntity(Guid id) : Entity<Guid>(id);
} 