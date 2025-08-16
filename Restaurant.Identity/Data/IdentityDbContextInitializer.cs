using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Common;
using Restaurant.Infrastructure.Common;
using Restaurant.Infrastructure.Entities;

namespace Restaurant.Infrastructure.Data;

public class IdentityDbContextInitializer(IdentityDbContext dbContext, 
    UserManager<ApplicationUser> userManager) : IIdentityDbContextInitializer
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

        if (!await dbContext.Roles.AnyAsync())
        {
            var applicationRoles = GetRoles();

            await dbContext.Roles.AddRangeAsync(applicationRoles);

            saveChanges = true;
        }

        if (!await dbContext.Users.AnyAsync())
        {
            var user = new ApplicationUser()
            {
                Id = "00000000-0000-0000-0000-000000000001",
                UserName = "Admin",
                Email = "Admin@Restaurant.com",
                FullName = "Admin",
                DateOfBirth = new DateOnly(2004, 9, 10),
            };

            await userManager.CreateAsync(user, "P@ssw0rd");
            await userManager.AddToRoleAsync(user, RoleTypes.Admin);
            await userManager.AddToRoleAsync(user, RoleTypes.Owner);
        }

        if(saveChanges)
            await dbContext.SaveChangesAsync();
    }
}