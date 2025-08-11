using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;

namespace Restaurant.Persistence.Data;

public sealed class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options): DbContext(options)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	internal DbSet<Domain.Entities.Restaurant> Restaurants { get; set; }
	internal DbSet<Dish> Dishes { get; set; }
}
