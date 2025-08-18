namespace Restaurant.Application.Test.Features.Dishes.Commands;

using System.Threading;
using System.Threading.Tasks;
using Moq;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Dishes.Commands.DeleteRestaurantDish;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;
using Restaurant.Application.Common.Enums;
using Restaurant.Domain.Specifications.Dishes;
using Xunit;
using RestaurantEntity = Domain.Entities.Restaurant;

public class DeleteRestaurantDishHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IGenericRepository<Dish, int>> _dishRepositoryMock = new();
    private readonly Mock<IGenericRepository<RestaurantEntity, int>> _restaurantRepositoryMock = new();
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock = new();

    public DeleteRestaurantDishHandlerTests()
    {
        _unitOfWorkMock
            .Setup(u => u.GetRepository<Dish, int>())
            .Returns(_dishRepositoryMock.Object);

        _unitOfWorkMock
            .Setup(u => u.GetRepository<RestaurantEntity, int>())
            .Returns(_restaurantRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRestaurantNotFound_ShouldThrowNotFoundException()
    {
        var command = new DeleteRestaurantDishCommand(RestaurantId: 1, Id: 5);
        _restaurantRepositoryMock
            .Setup(r => r.GetAsync(command.RestaurantId))
            .ReturnsAsync((RestaurantEntity?)null);

        var handler = new DeleteRestaurantDishCommandHandler(
            _unitOfWorkMock.Object,
            _restaurantAuthorizationServiceMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenUserNotAuthorized_ShouldThrowOperationForbiddenException()
    {
        var command = new DeleteRestaurantDishCommand(RestaurantId: 1, Id: 5);
        var restaurant = new RestaurantEntity();

        _restaurantRepositoryMock
            .Setup(r => r.GetAsync(command.RestaurantId))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock
            .Setup(s => s.IsAuthorized(restaurant, ResourceOperation.Delete))
            .Returns(false);

        var handler = new DeleteRestaurantDishCommandHandler(
            _unitOfWorkMock.Object,
            _restaurantAuthorizationServiceMock.Object);

        await Assert.ThrowsAsync<OperationForbiddenException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenDishNotFound_ShouldThrowNotFoundException()
    {
        var command = new DeleteRestaurantDishCommand(RestaurantId: 1, Id: 5);
        var restaurant = new RestaurantEntity();

        _restaurantRepositoryMock
            .Setup(r => r.GetAsync(command.RestaurantId))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock
            .Setup(s => s.IsAuthorized(restaurant, ResourceOperation.Delete))
            .Returns(true);

        _dishRepositoryMock
            .Setup(d => d.GetAsync(command.Id, It.IsAny<System.Linq.Expressions.Expression<System.Func<Dish, Dish>>>(), It.IsAny<GetDishByIdSpecification>()))
            .ReturnsAsync((Dish?)null);

        var handler = new DeleteRestaurantDishCommandHandler(
            _unitOfWorkMock.Object,
            _restaurantAuthorizationServiceMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldDeleteDishAndComplete()
    {
        var command = new DeleteRestaurantDishCommand(RestaurantId: 1, Id: 5);
        var restaurant = new RestaurantEntity();
        var dish = new Dish { Id = command.Id };

        _restaurantRepositoryMock
            .Setup(r => r.GetAsync(command.RestaurantId))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock
            .Setup(s => s.IsAuthorized(restaurant, ResourceOperation.Delete))
            .Returns(true);

        _dishRepositoryMock
            .Setup(d => d.GetAsync(command.Id, It.IsAny<System.Linq.Expressions.Expression<System.Func<Dish, Dish>>>(), It.IsAny<GetDishByIdSpecification>()))
            .ReturnsAsync(dish);

        var handler = new DeleteRestaurantDishCommandHandler(
            _unitOfWorkMock.Object,
            _restaurantAuthorizationServiceMock.Object);

        await handler.Handle(command, CancellationToken.None);

        _dishRepositoryMock.Verify(d => d.Delete(dish), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }
}
