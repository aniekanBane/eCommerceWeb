using System.Reflection;
using eCommerceWeb.Domain;
using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.Primitives.Events;
using FluentAssertions;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using NetArchTest.Rules;

namespace eCommerceWeb.UnitTests.Architecture;

public class DomainArchTests
{
    private static readonly Assembly domainAssembly = typeof(IDomainMarker).Assembly;

    [Fact]
    public void Classes_Should_NotHavePublicParameterlessConstructors()
    {
        // Act
        var result = Types.InAssembly(domainAssembly)
            .That()
            .DoNotResideInNamespaceStartingWith("eCommerceWeb.Domain.Primitives")
            .And().DoNotResideInNamespace("eCommerceWeb.Domain.Exceptions")
            .And().AreClasses()
            .And().AreNotNested()
            .And().AreNotAbstract()
            .And().DoNotHaveNameMatching(".*Builder$")
            .Should()
            .MeetCustomRule(new NotHavePublicParameterlessConstructorRule())
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entity_Primitives_Should_BeAbstract()
    {
        var result = Types.InAssembly(domainAssembly)
            .That()
            .ResideInNamespace(typeof(IEntity).Namespace)
            .Should()
            .BeAbstract()
            .GetResult();
        
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entities_ShouldHave_PrivateParameterlessConstructor()
    {
        // Act
        var result = Types.InAssembly(domainAssembly)
            .That()
            .ResideInNamespaceStartingWith("eCommerceWeb.Domain.Entities")
            .And().AreClasses()
            .And().AreNotNestedPrivate()
            .And().AreNotAbstract()
            .And().DoNotHaveNameEndingWith("Model")
            .Should()
            .MeetCustomRule(new HaveDefaultConstructorRule())
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entities_Should_BeSealed()
    {
        // Act
        var result = Types.InAssembly(domainAssembly)
            .That()
            .Inherit(typeof(Entity<>))
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entities_Should_HavePrivateSetters()
    {
        // Act
        var result = Types.InAssembly(domainAssembly)
            .That()
            .Inherit(typeof(Entity<>))
            .Should()
            .MeetCustomRule(new HaveNonPublicSettersRule())
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ValueObjects_Should_BeImmutable()
    {
        // Act
        var result = Types.InAssembly(domainAssembly)
            .That()
            .ResideInNamespaceContaining(nameof(Domain.ValueObjects))
            .Should()
            .BeImmutable()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ValueObjects_Should_HavePrivateParameterlessConstructor()
    {
        var result = Types.InAssembly(domainAssembly)
            .That()
            .ResideInNamespace("eCommerceWeb.Domain.ValueObjects")
            .And().AreNotAbstract()
            .Should()
            .MeetCustomRule(new HaveDefaultConstructorRule())
            .GetResult();
        
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainEvents_Follow_NamingConvention()
    {
        // Act
        var result = Types.InAssembly(domainAssembly)
            .That()
            .Inherit(typeof(DomainEvent))
            .And()
            .DoNotResideInNamespace("eCommerceWeb.Domain.Primitives")
            .Should()
            .HaveNameEndingWith("Event")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Enums_Should_BeSmartEnums()
    {
        // Act
        var result = Types.InAssembly(domainAssembly)
            .That()
            .HaveNameEndingWith("Type")
            .Or().HaveNameEndingWith("Status")
            .Or().HaveNameEndingWith("State")
            .Should()
            .BeSealed()
            .And().BeImmutable()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    private sealed class HaveDefaultConstructorRule : ICustomRule
    {
        public bool MeetsRule(TypeDefinition type)
        {
            return type.GetConstructors().Any(c => !c.IsPublic && c.Parameters.Count == 0);
        }
    }

    private sealed class HaveNonPublicSettersRule : ICustomRule
    {
        public bool MeetsRule(TypeDefinition type)
        {
            return type.Properties.Where(p => p.SetMethod is not null).All(p => !p.SetMethod.IsPublic);
        }
    }

    private sealed class NotHavePublicParameterlessConstructorRule : ICustomRule
    {
        public bool MeetsRule(TypeDefinition type)
        {
            return !type.GetConstructors().Any(c => c.IsPublic && c.Parameters.Count == 0);
        }
    }
}
