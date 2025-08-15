using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;

namespace Restaurant.Persistence.Data;

public sealed class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options): DbContext(options)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public DbSet<Domain.Entities.Restaurant> Restaurants { get; set; }
	public DbSet<Dish> Dishes { get; set; }
}
