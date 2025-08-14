using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Restaurant.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, config) =>
            config.ReadFrom.Configuration(context.Configuration));
    }
}