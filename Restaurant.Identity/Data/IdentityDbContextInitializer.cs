using Microsoft.EntityFrameworkCore;
using Restaurant.Infrastructure.Common;

namespace Restaurant.Infrastructure.Data;

public class IdentityDbContextInitializer(IdentityDbContext dbContext) : IIdentityDbContextInitializer
{
    public async Task InitializeAsync()    
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
            await dbContext.Database.MigrateAsync();
    }

    public Task SeedAsync()
    {
        return Task.CompletedTask;
    }
}