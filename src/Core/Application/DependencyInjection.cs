using System.Reflection;
using eCommerceWeb.Application.Pipeline.Behaviors;
using eCommerceWeb.Application.Pipeline.PreProcessors;
using FluentValidation;
using Mapster;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerceWeb.Application;

public static class DependencyInjection
{
    public static readonly Assembly AssemblyReference = typeof(DependencyInjection).Assembly;

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Mapster
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.EnableImmutableMapping();
        typeAdapterConfig.Scan(AssemblyReference);
        services.AddSingleton(typeAdapterConfig);

        services.AddValidatorsFromAssembly(AssemblyReference, includeInternalTypes: true);

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(AssemblyReference);
            cfg.AddRequestPreProcessor(typeof(IRequestPreProcessor<>), typeof(ValidationPreProcessor<>));
            cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
            cfg.AddOpenBehavior(typeof(RequestExceptionProcessorBehavior<,>));
        });
        
        return services;
    }
}
