using Moq;
using Restaurant.Application.Common.User;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Dishes.Commands.CreateRestaurantDish;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Test.Features.Dishes.Commands;
using RestaurantEntity = Domain.Entities.Restaurant;

public class CreateRestaurantDishCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IGenericRepository<Dish, int>> _dishRepoMock;
    private readonly Mock<IGenericRepository<RestaurantEntity, int>> _restaurantRepoMock;

    private readonly CreateRestaurantDishCommandHandler _handler;

    public CreateRestaurantDishCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userServiceMock = new Mock<IUserService>();
        _dishRepoMock = new Mock<IGenericRepository<Dish, int>>();
        _restaurantRepoMock = new Mock<IGenericRepository<RestaurantEntity, int>>();

        _unitOfWorkMock
            .Setup(x => x.GetRepository<Dish, int>())
            .Returns(_dishRepoMock.Object);

        _unitOfWorkMock
            .Setup(x => x.GetRepository<RestaurantEntity, int>())
            .Returns(_restaurantRepoMock.Object);

        _handler = new CreateRestaurantDishCommandHandler(_unitOfWorkMock.Object, _userServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRestaurantNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        _restaurantRepoMock
            .Setup(x => x.GetAsync(It.IsAny<int>()))
            .ReturnsAsync((RestaurantEntity?)null);

        var command = new CreateRestaurantDishCommand(
            RestaurantId: 1,
            Name: "Pizza",
            Description: "Cheese pizza",
            Price: 50m,
            KiloCalories: 300
        );

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenUserNotOwner_ShouldThrowOperationForbiddenException()
    {
        // Arrange
        var restaurant = new RestaurantEntity { Id = 1, OwnerId = "2" };

        _restaurantRepoMock
            .Setup(x => x.GetAsync(1))
            .ReturnsAsync(restaurant);

        _userServiceMock
            .Setup(x => x.GetCurrentUser())
            .Returns(new CurrentUser() { Id = "3" });

        var command = new CreateRestaurantDishCommand(
            RestaurantId: 1,
            Name: "Burger",
            Description: "Beef burger",
            Price: 70m,
            KiloCalories: 400
        );

        // Act & Assert
        await Assert.ThrowsAsync<OperationForbiddenException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldCreateDishAndReturnId()
    {
        // Arrange
        var restaurant = new RestaurantEntity { Id = 1, OwnerId = "5" };
        var user = new CurrentUser() { Id = "5" };

        _restaurantRepoMock
            .Setup(x => x.GetAsync(1))
            .ReturnsAsync(restaurant);

        _userServiceMock
            .Setup(x => x.GetCurrentUser())
            .Returns(user);

        _dishRepoMock
            .Setup(x => x.CreateAsync(It.IsAny<Dish>()))
            .Returns(ValueTask.CompletedTask)
            .Callback<Dish>(d => d.Id = 10);

        _unitOfWorkMock
            .Setup(x =>  x.CompleteAsync())
            .Returns(Task.FromResult(1));

        var command = new CreateRestaurantDishCommand(
            RestaurantId: 1,
            Name: "Pasta",
            Description: "Creamy pasta",
            Price: 90m,
            KiloCalories: 500
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(10, result);
        _dishRepoMock.Verify(x => x.CreateAsync(It.IsAny<Dish>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CompleteAsync(), Times.Once);
    }
}
