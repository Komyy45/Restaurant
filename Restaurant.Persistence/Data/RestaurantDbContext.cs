using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;

namespace Restaurant.Persistence.Data;

internal class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options): DbContext(options)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyInformation).Assembly);
	}

	internal DbSet<Domain.Entities.Restaurant> Restaurants { get; set; }
	internal DbSet<Dish> Dishes { get; set; }
}
