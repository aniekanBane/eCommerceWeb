using eCommerceWeb.Application.Pipeline.Behaviors;
using eCommerceWeb.Application.Pipeline.PreProcessors;
using FluentValidation;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerceWeb.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(IApplicationMarker).Assembly, includeInternalTypes: true);

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(IApplicationMarker).Assembly);
            cfg.AddRequestPreProcessor(typeof(IRequestPreProcessor<>), typeof(ValidationPreProcessor<>));
            cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
            cfg.AddOpenBehavior(typeof(RequestExceptionProcessorBehavior<,>));
        });
        
        return services;
    }
}
