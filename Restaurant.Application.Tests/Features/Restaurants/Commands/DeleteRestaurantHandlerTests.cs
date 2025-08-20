using FluentAssertions;
using Moq;
using Restaurant.Application.Common.Enums;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Restaurant.Commands.DeleteRestaurant;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Test.Features.Restaurants.Commands;

public class DeleteRestaurantHandlerTests
{
    private readonly Mock<IRestaurantAuthorizationService> _authorizationService;
    private readonly Mock<IGenericRepository<Domain.Entities.Restaurant, int>> _genericRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public DeleteRestaurantHandlerTests()
    {
        _authorizationService = new Mock<IRestaurantAuthorizationService>();
        _genericRepository = new Mock<IGenericRepository<Domain.Entities.Restaurant, int>>();
        _unitOfWork = new Mock<IUnitOfWork>();

        _unitOfWork.Setup(u => u.GetRepository<Domain.Entities.Restaurant, int>())
                   .Returns(_genericRepository.Object);

        _unitOfWork.Setup(u => u.CompleteAsync())
                   .ReturnsAsync(1);
    }

    private DeleteRestaurantCommandHandler CreateHandler() =>
        new DeleteRestaurantCommandHandler(_unitOfWork.Object, _authorizationService.Object);

    [Fact]
    public async Task Handle_RestaurantNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new DeleteRestaurantCommand(1);

        _genericRepository.Setup(r => r.GetAsync(It.IsAny<int>()))
                          .ReturnsAsync((Domain.Entities.Restaurant)null);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        _genericRepository.Verify(r => r.GetAsync(1), Times.Once);
        _genericRepository.Verify(r => r.Delete(It.IsAny<Domain.Entities.Restaurant>()), Times.Never);
        _unitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_RestaurantNotAuthorized_ThrowsOperationForbiddenException()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new DeleteRestaurantCommand(1);
        var restaurant = new Domain.Entities.Restaurant { Id = 1 };

        _genericRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(restaurant);
        _authorizationService.Setup(a => a.IsAuthorized(restaurant, ResourceOperation.Delete))
                             .Returns(false);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<OperationForbiddenException>();

        _genericRepository.Verify(r => r.GetAsync(1), Times.Once);
        _genericRepository.Verify(r => r.Delete(It.IsAny<Domain.Entities.Restaurant>()), Times.Never);
        _unitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        _authorizationService.Verify(a => a.IsAuthorized(restaurant, ResourceOperation.Delete), Times.Once);
    }

    [Fact]
    public async Task Handle_RestaurantAuthorized_DeletesAndCompletes()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new DeleteRestaurantCommand(1);
        var restaurant = new Domain.Entities.Restaurant { Id = 1 };

        _genericRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(restaurant);
        _authorizationService.Setup(a => a.IsAuthorized(restaurant, ResourceOperation.Delete))
                             .Returns(true);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _genericRepository.Verify(r => r.GetAsync(1), Times.Once);
        _authorizationService.Verify(a => a.IsAuthorized(restaurant, ResourceOperation.Delete), Times.Once);
        _genericRepository.Verify(r => r.Delete(restaurant), Times.Once);
        _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
    }
}
