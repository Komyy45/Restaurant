using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Application.Common.Behaviors;

namespace Restaurant.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.AddMediatR(config => 
            config.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true)
            .AddFluentValidationAutoValidation();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        
        return services;
    }
}