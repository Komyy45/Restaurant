using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Common;
using Restaurant.Infrastructure.Common;

namespace Restaurant.Infrastructure.Data;

public class IdentityDbContextInitializer(IdentityDbContext dbContext) : IIdentityDbContextInitializer
{
    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles = [
            new() { Name = RoleTypes.Admin, NormalizedName = RoleTypes.Admin.ToUpper() },
            new() { Name = RoleTypes.Owner, NormalizedName = RoleTypes.Owner.ToUpper() },
            new() { Name = RoleTypes.Customer, NormalizedName = RoleTypes.Customer.ToUpper() }
        ];

        return roles;
    }
    
    public async Task InitializeAsync()    
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
            await dbContext.Database.MigrateAsync();
    }

    public async Task SeedAsync()
    {
        bool saveChanges = false;

        if (!dbContext.Roles.Any())
        {
            var applicationRoles = GetRoles();

            await dbContext.Roles.AddRangeAsync(applicationRoles);

            saveChanges = true;
        }

        if(saveChanges)
            await dbContext.SaveChangesAsync();
    }
}