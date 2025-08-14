using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using Restaurant.Persistence.Identity;

namespace Restaurant.Persistence.Data;

public sealed class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options): IdentityDbContext<ApplicationUser>(options)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	internal DbSet<Domain.Entities.Restaurant> Restaurants { get; set; }
	internal DbSet<Dish> Dishes { get; set; }
}
