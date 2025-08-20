using Moq;
using Restaurant.Application.Common.Enums;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Dishes.Commands.UpdateRestaurantDish;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;
using RestaurantEntity = Restaurant.Domain.Entities.Restaurant;

namespace Restaurant.Application.Test.Features.Dishes.Commands;

public class UpdateRestaurantDishCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<Dish, int>> _dishRepositoryMock;
    private readonly Mock<IGenericRepository<RestaurantEntity, int>> _restaurantRepositoryMock;
    private readonly Mock<IRestaurantAuthorizationService> _authServiceMock;
    private readonly UpdateRestaurantDishCommandHandler _handler;

    public UpdateRestaurantDishCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dishRepositoryMock = new Mock<IGenericRepository<Dish, int>>();
        _restaurantRepositoryMock = new Mock<IGenericRepository<RestaurantEntity, int>>();
        _authServiceMock = new Mock<IRestaurantAuthorizationService>();

        _unitOfWorkMock.Setup(x => x.GetRepository<Dish, int>())
            .Returns(_dishRepositoryMock.Object);
        _unitOfWorkMock.Setup(x => x.GetRepository<RestaurantEntity, int>())
            .Returns(_restaurantRepositoryMock.Object);

        _handler = new UpdateRestaurantDishCommandHandler(
            _unitOfWorkMock.Object,
            _authServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenRestaurantDoesNotExist_ShouldThrowNotFoundException()
    {
        var command = new UpdateRestaurantDishCommand(
            Id: 1,
            RestaurantId: 100,
            Name: "Pizza",
            Description: "Cheese",
            Price: 10,
            KiloCalories: 200
        );

        _restaurantRepositoryMock
            .Setup(r => r.GetAsync(command.RestaurantId))
            .ReturnsAsync((RestaurantEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenUserNotAuthorized_ShouldThrowOperationForbiddenException()
    {
        var restaurant = new RestaurantEntity { Id = 100 };
        var command = new UpdateRestaurantDishCommand(
            Id: 1,
            RestaurantId: 100,
            Name: "Pizza",
            Description: "Cheese",
            Price: 10,
            KiloCalories: 200
        );

        _restaurantRepositoryMock
            .Setup(r => r.GetAsync(command.RestaurantId))
            .ReturnsAsync(restaurant);

        _authServiceMock
            .Setup(a => a.IsAuthorized(restaurant, ResourceOperation.Update))
            .Returns(false);

        await Assert.ThrowsAsync<OperationForbiddenException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenDishDoesNotExist_ShouldThrowNotFoundException()
    {
        var restaurant = new RestaurantEntity { Id = 100 };
        var command = new UpdateRestaurantDishCommand(
            Id: 1,
            RestaurantId: 100,
            Name: "Pizza",
            Description: "Cheese",
            Price: 10,
            KiloCalories: 200
        );

        _restaurantRepositoryMock
            .Setup(r => r.GetAsync(command.RestaurantId))
            .ReturnsAsync(restaurant);

        _authServiceMock
            .Setup(a => a.IsAuthorized(restaurant, ResourceOperation.Update))
            .Returns(true);

        _dishRepositoryMock
            .Setup(d => d.GetAsync(command.Id))
            .ReturnsAsync((Dish?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenDishRestaurantMismatch_ShouldThrowNotFoundException()
    {
        var restaurant = new RestaurantEntity { Id = 100 };
        var dish = new Dish { Id = 1, RestaurantId = 200 };
        var command = new UpdateRestaurantDishCommand(
            Id: 1,
            RestaurantId: 100,
            Name: "Pizza",
            Description: "Cheese",
            Price: 10,
            KiloCalories: 200
        );

        _restaurantRepositoryMock
            .Setup(r => r.GetAsync(command.RestaurantId))
            .ReturnsAsync(restaurant);

        _authServiceMock
            .Setup(a => a.IsAuthorized(restaurant, ResourceOperation.Update))
            .Returns(true);

        _dishRepositoryMock
            .Setup(d => d.GetAsync(command.Id))
            .ReturnsAsync(dish);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenValid_ShouldUpdateDishAndCompleteUnitOfWork()
    {
        var restaurant = new RestaurantEntity { Id = 100 };
        var dish = new Dish
        {
            Id = 1,
            RestaurantId = 100,
            Name = "Old",
            Description = "OldDesc",
            Price = 5,
            KiloCalories = 100
        };

        var command = new UpdateRestaurantDishCommand(
            Id: 1,
            RestaurantId: 100,
            Name: "Pizza",
            Description: "Cheese",
            Price: 10,
            KiloCalories: 200
        );

        _restaurantRepositoryMock
            .Setup(r => r.GetAsync(command.RestaurantId))
            .ReturnsAsync(restaurant);

        _authServiceMock
            .Setup(a => a.IsAuthorized(restaurant, ResourceOperation.Update))
            .Returns(true);

        _dishRepositoryMock
            .Setup(d => d.GetAsync(command.Id))
            .ReturnsAsync(dish);

        await _handler.Handle(command, CancellationToken.None);

        _dishRepositoryMock.Verify(d => d.Update(It.Is<Dish>(
            updated => updated.Name == command.Name &&
                       updated.Description == command.Description &&
                       updated.Price == command.Price &&
                       updated.KiloCalories == command.KiloCalories
        )), Times.Once);

        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }
}
