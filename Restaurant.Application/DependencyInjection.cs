using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Restaurant.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.AddMediatR(config => 
            config.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly)
            .AddFluentValidationAutoValidation();
        
        return services;
    }
}