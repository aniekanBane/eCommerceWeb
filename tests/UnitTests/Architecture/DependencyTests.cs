using System.Reflection;
using eCommerceWeb.Application;
using eCommerceWeb.Domain;
using eCommerceWeb.External;
using eCommerceWeb.Persistence;
using FluentAssertions;
using NetArchTest.Rules;

namespace eCommerceWeb.UnitTests.Architecture;

public class DependencyTests
{
    public static readonly Assembly DomainAssembly = typeof(IDomainMarker).Assembly;
    public static readonly Assembly ApplicationAssembly = typeof(IApplicationMarker).Assembly;
    public static readonly Assembly PersistenceAssembly = typeof(IPersistenceMarker).Assembly;
    public static readonly Assembly ExternalAssembly = typeof(IExternalServicesMarker).Assembly;
    
    [Fact]
    public void Domain_ShouldNotHaveAnyDependency()
    {
        // Act
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                nameof(Application),
                nameof(Persistence),
                nameof(External),
                nameof(WebApp),
                nameof(PublicApi)
            )
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_ShouldNotHaveDependencyOn_Infrastructure_Or_Presentation()
    {
        // Act
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                nameof(Persistence),
                nameof(External),
                nameof(PublicApi),
                nameof(WebApp)
            )
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_ShouldOnlyDependOn_Application_And_Domain()
    {
        // Act
        var result = Types.InAssemblies([PersistenceAssembly, ExternalAssembly])
            .Should()
            .NotHaveDependencyOnAny(
                nameof(PublicApi),
                nameof(WebApp)
            )
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
