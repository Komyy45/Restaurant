using FluentAssertions;
using Moq;
using Restaurant.Application.Common.Enums;
using Restaurant.Application.Common.User;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Restaurant.Commands.CreateRestaurant;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Test.Features.Restaurants.Commands;

public class CreateRestaurantHandlerTests
{
    private readonly Mock<IRestaurantAuthorizationService> _authorizationService;
    private readonly Mock<IGenericRepository<Domain.Entities.Restaurant, int>> _genericRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserService> _userService;

    public CreateRestaurantHandlerTests()
    {
        // Arrange shared mocks
        _authorizationService = new Mock<IRestaurantAuthorizationService>();
        _genericRepository = new Mock<IGenericRepository<Domain.Entities.Restaurant, int>>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _userService = new Mock<IUserService>();

        _genericRepository.Setup(r => r.CreateAsync(It.IsAny<Domain.Entities.Restaurant>()));

        _unitOfWork.Setup(u => u.GetRepository<Domain.Entities.Restaurant, int>())
                   .Returns(_genericRepository.Object);

        _unitOfWork.Setup(u => u.CompleteAsync())
                   .ReturnsAsync(1);
    }

    private CreateRestaurantCommandHandler CreateHandler() =>
        new CreateRestaurantCommandHandler(_unitOfWork.Object, _authorizationService.Object, _userService.Object);

    private CreateRestaurantCommand CreateCommand() =>
        new CreateRestaurantCommand(
            Name: "KFC",
            Description: "Description",
            Category: "Category",
            HasDelivery: false,
            ContactEmail: "ContactEmail",
            ContactNumber: "ContactNumber",
            City: "City",
            Street: "Street",
            PostalCode: "PostalCode"
        );

    [Fact]
    public async Task Handle_IsAuthorized_ReturnsCreatedRestaurantId()
    {
        // Arrange
        _authorizationService.Setup(a => a.IsAuthorized(It.IsAny<Domain.Entities.Restaurant>(), ResourceOperation.Create))
                             .Returns(true);

        _userService.Setup(u => u.GetCurrentUser())
            .Returns(new CurrentUser
            {
                Email = "test@gmail.com",
                Id = "1",
                Roles = new[] { "Admin", "Owner" },
                UserName = "test"
            });

        var handler = CreateHandler();
        var command = CreateCommand();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _userService.Verify(m => m.GetCurrentUser(), Times.Once);
        _unitOfWork.Verify(m => m.GetRepository<Domain.Entities.Restaurant, int>(), Times.Once);
        _authorizationService.Verify(m => m.IsAuthorized(It.IsAny<Domain.Entities.Restaurant>(), It.IsAny<ResourceOperation>()), Times.Once);
        _genericRepository.Verify(m => m.CreateAsync(It.IsAny<Domain.Entities.Restaurant>()), Times.Once);
        _unitOfWork.Verify(m => m.CompleteAsync(), Times.Once);

        result.Should().BeOfType(typeof(int));
    }

    [Fact]
    public async Task Handle_IsNotAuthorized_ThrowsOperationForbiddenException()
    {
        // Arrange
        _authorizationService.Setup(a => a.IsAuthorized(It.IsAny<Domain.Entities.Restaurant>(), It.IsAny<ResourceOperation>()))
                             .Returns(false);

        _userService.Setup(u => u.GetCurrentUser())
            .Returns(new CurrentUser
            {
                Email = "test@gmail.com",
                Id = "1",
                Roles = new[] { "Admin" },
                UserName = "test"
            });

        var handler = CreateHandler();
        var command = CreateCommand();

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<OperationForbiddenException>();

        _userService.Verify(m => m.GetCurrentUser(), Times.Once);
        _unitOfWork.Verify(m => m.GetRepository<Domain.Entities.Restaurant, int>(), Times.Once);
        _authorizationService.Verify(m => m.IsAuthorized(It.IsAny<Domain.Entities.Restaurant>(), It.IsAny<ResourceOperation>()), Times.Once);
        _genericRepository.Verify(m => m.CreateAsync(It.IsAny<Domain.Entities.Restaurant>()), Times.Never);
        _unitOfWork.Verify(m => m.CompleteAsync(), Times.Never);
    }
}
