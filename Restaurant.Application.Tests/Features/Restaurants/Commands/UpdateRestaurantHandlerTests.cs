using FluentAssertions;
using Moq;
using Restaurant.Application.Common.Enums;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Restaurant.Commands.UpdateRestaurant;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;

using RestaurantEntity = Restaurant.Domain.Entities.Restaurant;

namespace Restaurant.Application.Test.Features.Restaurants.Commands;

public class UpdateRestaurantHandlerTests
{
    private readonly Mock<IRestaurantAuthorizationService> _authorizationService;
    private readonly Mock<IGenericRepository<RestaurantEntity, int>> _genericRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateRestaurantHandlerTests()
    {
        // Arrange shared mocks
        _authorizationService = new Mock<IRestaurantAuthorizationService>();
        _genericRepository = new Mock<IGenericRepository<RestaurantEntity, int>>();
        _unitOfWork = new Mock<IUnitOfWork>();

        _unitOfWork.Setup(u => u.GetRepository<RestaurantEntity, int>())
                   .Returns(_genericRepository.Object);

        _unitOfWork.Setup(u => u.CompleteAsync())
                   .ReturnsAsync(1);

        _genericRepository.Setup(r => r.Update(It.IsAny<RestaurantEntity>()));
    }

    private UpdateRestaurantCommandHandler CreateHandler() =>
        new UpdateRestaurantCommandHandler(_unitOfWork.Object, _authorizationService.Object);

    private UpdateRestaurantCommand CreateCommand() =>
        new UpdateRestaurantCommand(
            Id: 1,
            Name: "UpdatedName",
            Description: "UpdatedDescription",
            HasDelivery: true
        );

    [Fact]
    public async Task Handle_RestaurantNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var handler = CreateHandler();
        var command = CreateCommand();

        _genericRepository.Setup(r => r.GetAsync(command.Id))
                          .ReturnsAsync((RestaurantEntity)null);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _genericRepository.Verify(r => r.GetAsync(command.Id), Times.Once);
        _genericRepository.Verify(r => r.Update(It.IsAny<RestaurantEntity>()), Times.Never);
        _unitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_IsNotAuthorized_ThrowsOperationForbiddenException()
    {
        // Arrange
        var restaurant = new RestaurantEntity
        {
            Id = 1
        };
        var handler = CreateHandler();
        var command = CreateCommand();

        _genericRepository.Setup(r => r.GetAsync(command.Id))
                          .ReturnsAsync(restaurant);

        _authorizationService.Setup(a => a.IsAuthorized(restaurant, ResourceOperation.Update))
                             .Returns(false);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<OperationForbiddenException>();
        _genericRepository.Verify(r => r.GetAsync(command.Id), Times.Once);
        _authorizationService.Verify(a => a.IsAuthorized(restaurant, ResourceOperation.Update), Times.Once);
        _genericRepository.Verify(r => r.Update(It.IsAny<RestaurantEntity>()), Times.Never);
        _unitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_IsAuthorized_UpdatesAndCompletes()
    {
        // Arrange
        var restaurant = new RestaurantEntity
        {
            Id = 1, Name = "test", Category = "test", HasDelivery = false, ContactEmail = "test@gmail.com",
            Description = "test", Address = new Address() { City = "test", PostalCode = "test", Street = "test" }
        };
        var handler = CreateHandler();
        var command = CreateCommand();

        _genericRepository.Setup(r => r.GetAsync(command.Id))
                          .ReturnsAsync(restaurant);

        _authorizationService.Setup(a => a.IsAuthorized(restaurant, ResourceOperation.Update))
                             .Returns(true);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _genericRepository.Verify(r => r.GetAsync(command.Id), Times.Once);
        _authorizationService.Verify(a => a.IsAuthorized(restaurant, ResourceOperation.Update), Times.Once);
        _genericRepository.Verify(r => r.Update(It.IsAny<RestaurantEntity>()), Times.Once);
        _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
    }
}
