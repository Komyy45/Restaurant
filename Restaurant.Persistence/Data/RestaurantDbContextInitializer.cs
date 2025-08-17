using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using RestaurantEntity = Restaurant.Domain.Entities.Restaurant;

namespace Restaurant.Persistence.Data;
public sealed class RestaurantDbContextInitializer(RestaurantDbContext dbContext) : IRestaurantDbContextInitializer
{
	private readonly string _ownerId = "00000000-0000-0000-0000-000000000001";
	
	
	private IEnumerable<RestaurantEntity> GetRestaurants() => new List<RestaurantEntity>
	{
		new RestaurantEntity
		{
			Name = "Bella Italia",
			Description = "Authentic Italian cuisine with fresh pasta and wood-fired pizza.",
			Category = "Italian",
			HasDelivery = true,
			ContactEmail = "info@bellaitalia.com",
			ContactNumber = "+1-202-555-0111",
			Address = new Address
			{
				Street = "123 Pasta St",
				City = "New York",
				PostalCode = "10001",
			},
			Dishes = new List<Dish>
			{
				new() { Name = "Margherita Pizza", Description = "Tomato, mozzarella, fresh basil", Price = 12.99m, KiloCalories = 850, RestaurantId = 1 },
				new() { Name = "Fettuccine Alfredo", Description = "Creamy parmesan sauce with fresh pasta", Price = 14.50m, KiloCalories = 950, RestaurantId = 1 }
			},
			OwnerId = _ownerId
		},
		new RestaurantEntity
		{
			Name = "Sushi World",
			Description = "Fresh sushi and sashimi prepared by master chefs.",
			Category = "Japanese",
			HasDelivery = false,
			ContactEmail = "contact@sushiworld.com",
			ContactNumber = "+1-202-555-0222",
			Address = new Address
			{
				Street = "456 Ocean Ave",
				City = "San Francisco",
				PostalCode = "94109",
			},
			Dishes = new List<Dish>
			{
				new() { Name = "Salmon Nigiri", Description = "Fresh salmon over sushi rice", Price = 4.50m, KiloCalories = 200, RestaurantId = 2 },
				new() { Name = "California Roll", Description = "Crab, avocado, cucumber", Price = 6.99m, KiloCalories = 300, RestaurantId = 2 }
			},
			OwnerId = _ownerId
		},
		new RestaurantEntity
		{
			Name = "Spice Garden",
			Description = "Traditional Indian food with aromatic spices.",
			Category = "Indian",
			HasDelivery = true,
			ContactEmail = "hello@spicegarden.com",
			ContactNumber = "+1-202-555-0333",
			Address = new Address
			{
				Street = "789 Curry Rd",
				City = "Chicago",
				PostalCode = "60616",
			},
			Dishes = new List<Dish>
			{
				new() { Name = "Chicken Tikka Masala", Description = "Creamy tomato sauce with tender chicken", Price = 13.99m, KiloCalories = 750, RestaurantId = 3 },
				new() { Name = "Garlic Naan", Description = "Soft Indian bread with garlic butter", Price = 3.50m, KiloCalories = 250, RestaurantId = 3 }
			},
			OwnerId = _ownerId
		}
	};

	public async Task InitializeAsync()
	{
		var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

		if (pendingMigrations.Any())
			 await dbContext.Database.MigrateAsync();
	}

	public async Task SeedAsync()
	{
		if(await dbContext.Database.CanConnectAsync())
		{
			bool isSavingChanges = false;

			if(!dbContext.Restaurants.Any())
			{
				var restaurants = GetRestaurants();
				await dbContext.AddRangeAsync(restaurants);
				isSavingChanges = true;
			}
			
			if (isSavingChanges) await dbContext.SaveChangesAsync();
		}
	}
}
