using FluentAssertions;
using Restaurant.Application.Features.Dishes.Commands.CreateRestaurantDish;
using Restaurant.Application.Features.Dishes.Commands.UpdateRestaurantDish;
using Restaurant.Application.Mapping;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Test.Mapping
{
    public class DishesMappingProfileTests
    {
        [Fact]
        public void Dish_ToDto_MapsCorrectly()
        {
            var dish = new Dish
            {
                Id = 1,
                Name = "Pasta",
                Description = "Delicious Italian pasta",
                Price = 12.5m,
                KiloCalories = 450,
                RestaurantId = 100
            };

            var result = dish.ToDto();

            result.Should().NotBeNull();
            result.Id.Should().Be(dish.Id);
            result.Name.Should().Be(dish.Name);
            result.Description.Should().Be(dish.Description);
            result.Price.Should().Be(dish.Price);
            result.KiloCalories.Should().Be(dish.KiloCalories);
            result.RestaurantId.Should().Be(dish.RestaurantId);
        }

        [Fact]
        public void CreateRestaurantDishCommand_ToEntity_MapsCorrectly()
        {
            var command = new CreateRestaurantDishCommand(
                Name: "Pizza",
                Description: "Cheesy pizza",
                Price: 9.99m,
                KiloCalories: 600,
                RestaurantId: 200
            );

            var result = command.ToEntity();

            result.Should().NotBeNull();
            result.Name.Should().Be(command.Name);
            result.Description.Should().Be(command.Description);
            result.Price.Should().Be(command.Price);
            result.KiloCalories.Should().Be(command.KiloCalories);
            result.RestaurantId.Should().Be(command.RestaurantId);
        }

        [Fact]
        public void UpdateRestaurantDishCommand_ToEntity_MapsCorrectly()
        {
            var command = new UpdateRestaurantDishCommand(
                Id: 5,
                Name: "Updated Burger",
                Description: "Juicy beef burger",
                Price: 15.75m,
                KiloCalories: 700,
                RestaurantId: 300
            );

            var existingEntity = new Dish
            {
                Id = 5,
                Name = "Old Burger",
                Description = "Dry burger",
                Price = 10.0m,
                KiloCalories = 500,
                RestaurantId = 300
            };

            var result = command.ToEntity(existingEntity);

            result.Should().NotBeNull();
            result.Id.Should().Be(existingEntity.Id); // preserve Id
            result.Name.Should().Be(command.Name);
            result.Description.Should().Be(command.Description);
            result.Price.Should().Be(command.Price);
            result.KiloCalories.Should().Be(command.KiloCalories);
            result.RestaurantId.Should().Be(command.RestaurantId);
        }
    }
}
