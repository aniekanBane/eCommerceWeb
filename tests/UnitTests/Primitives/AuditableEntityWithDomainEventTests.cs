using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.Primitives.Events;
using FluentAssertions;

namespace eCommerceWeb.UnitTests.Primitives;

public class AuditableEntityWithDomainEventTests
{
    [Fact]
    public void DomainEvents_WhenCreated_ShouldBeEmpty()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid());

        // Assert
        entity.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void RaiseDomainEvent_ShouldAddEventToCollection()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid());
        var domainEvent = new TestDomainEvent();

        // Act
        entity.RaiseDomainEvent(domainEvent);

        // Assert
        entity.DomainEvents.Should().ContainSingle()
            .Which.Should().Be(domainEvent);
    }

    [Fact]
    public void RaiseDomainEvent_WithMultipleEvents_ShouldMaintainOrder()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid());
        var event1 = new TestDomainEvent();
        var event2 = new TestDomainEvent();
        var event3 = new TestDomainEvent();

        // Act
        entity.RaiseDomainEvent(event1);
        entity.RaiseDomainEvent(event2);
        entity.RaiseDomainEvent(event3);

        // Assert
        entity.DomainEvents.Should().HaveCount(3);
        entity.DomainEvents.Should().ContainInOrder(event1, event2, event3);
    }

    [Fact]
    public void RemoveDomainEvent_ShouldRemoveSpecificEvent()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid());
        var eventToKeep = new TestDomainEvent();
        var eventToRemove = new TestDomainEvent();
        entity.RaiseDomainEvent(eventToKeep);
        entity.RaiseDomainEvent(eventToRemove);

        // Act
        entity.RemoveDomainEvent(eventToRemove);

        // Assert
        entity.DomainEvents.Should().ContainSingle()
            .Which.Should().Be(eventToKeep);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid());
        entity.RaiseDomainEvent(new TestDomainEvent());
        entity.RaiseDomainEvent(new TestDomainEvent());
        entity.RaiseDomainEvent(new TestDomainEvent());

        // Act
        entity.ClearDomainEvents();

        // Assert
        entity.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void DomainEvents_ShouldBeReadOnly()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid());
        var domainEvent = new TestDomainEvent();

        // Act
        entity.RaiseDomainEvent(domainEvent);

        // Assert
        entity.DomainEvents.Should().BeOfType<System.Collections.ObjectModel.ReadOnlyCollection<DomainEvent>>();
        FluentActions.Invoking(() => 
            ((ICollection<DomainEvent>)entity.DomainEvents).Add(new TestDomainEvent()))
            .Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void RemoveDomainEvent_WhenEventDoesNotExist_ShouldNotThrow()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid());
        var nonExistentEvent = new TestDomainEvent();

        // Act & Assert
        FluentActions.Invoking(() => entity.RemoveDomainEvent(nonExistentEvent))
            .Should().NotThrow();
    }

    private class TestAuditableEntity : AuditableEntityWithDomainEvent<Guid>
    {
        public TestAuditableEntity(Guid id) : base() => Id = id;
    }

    private class TestDomainEvent : DomainEvent;
} 