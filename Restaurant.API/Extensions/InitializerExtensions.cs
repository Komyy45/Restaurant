using Restaurant.Application.Contracts;
using Restaurant.Infrastructure.Common;
using Restaurant.Persistence.Data;

namespace Restaurant.API.Extensions;

public static class InitializerExtensions
{
    public static async Task InitializeAsync<TInitializer>(this WebApplication app) 
        where TInitializer : IDbContextInitializer
    {
        using var scope = app.Services.CreateScope();
        try
        {
            var identityDbContextInitializer = scope.ServiceProvider.GetRequiredService<TInitializer>();

            await identityDbContextInitializer.InitializeAsync();
            await identityDbContextInitializer.SeedAsync();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            logger.LogError(ex.Message);
        }
    }
} 