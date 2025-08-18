using Moq;
using Restaurant.Application.Features.Dishes.Queries.GetDishById;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;
using Restaurant.Domain.Specifications.Dishes;

namespace Restaurant.Application.Test.Features.Dishes.Queries;

public sealed class GetDishByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<Dish, int>> _dishRepositoryMock;
    private readonly GetDishByIdQueryHandler _handler;

    public GetDishByIdQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dishRepositoryMock = new Mock<IGenericRepository<Dish, int>>();

        _unitOfWorkMock.Setup(u => u.GetRepository<Dish, int>())
            .Returns(_dishRepositoryMock.Object);

        _handler = new GetDishByIdQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WhenDishExists_ShouldReturnDishResponse()
    {
        // Arrange
        var query = new GetDishByIdQuery(RestaurantId: 1, Id: 10);
        var dish = new Dish
        {
            Id = query.Id,
            RestaurantId = query.RestaurantId,
            Name = "Pizza",
            Description = "Cheese Pizza",
            Price = 50
        };

        _dishRepositoryMock
            .Setup(r => r.GetAsync(query.Id, It.IsAny<GetDishByIdSpecification>()))
            .ReturnsAsync(dish);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dish.Id, result.Id);
        Assert.Equal(dish.Name, result.Name);
        _dishRepositoryMock.Verify(r => r.GetAsync(query.Id, It.IsAny<GetDishByIdSpecification>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDishDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var query = new GetDishByIdQuery(RestaurantId: 1, Id: 999);

        _dishRepositoryMock
            .Setup(r => r.GetAsync(query.Id, It.IsAny<GetDishByIdSpecification>()))
            .ReturnsAsync((Dish?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(query, CancellationToken.None));
    }
}
