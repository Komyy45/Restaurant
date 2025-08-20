using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Restaurant.Application.Features.Restaurant.Queries.GetAllRestaurants;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Specifications.Restaurants;
using RestaurantEntity = Restaurant.Domain.Entities.Restaurant;

namespace Restaurant.Application.Test.Features.Restaurants.Queries;

public class GetAllRestaurantsHandlerTests
{
    private readonly Mock<IGenericRepository<RestaurantEntity, int>> _genericRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public GetAllRestaurantsHandlerTests()
    {
        // Arrange shared mocks
        _genericRepository = new Mock<IGenericRepository<RestaurantEntity, int>>();
        _unitOfWork = new Mock<IUnitOfWork>();

        _unitOfWork.Setup(u => u.GetRepository<RestaurantEntity, int>())
                   .Returns(_genericRepository.Object);
    }

    private GetAllRestaurantsQueryHandler CreateHandler() =>
        new GetAllRestaurantsQueryHandler(_unitOfWork.Object);

    private GetAllRestaurantsQuery CreateQuery() =>
        new GetAllRestaurantsQuery(
            SearchText: "KFC",
            PageSize: 10,
            PageNumber: 1,
            SortBy: "Name",
            SortDirection: true
        );

    [Fact]
    public async Task Handle_ReturnsPaginatedRestaurants()
    {
        // Arrange
        var handler = CreateHandler();
        var query = CreateQuery();

        var restaurants = new List<RestaurantEntity>
        {
            new RestaurantEntity { Id = 1, Name = "KFC" },
            new RestaurantEntity { Id = 2, Name = "McDonalds" }
        };

        _genericRepository.Setup(r => r.GetAllAsync(It.IsAny<GetAllRestaurantsSpecification>(), true))
                          .ReturnsAsync(restaurants);

        _genericRepository.Setup(r => 
                r.CountAsync(null))
                          .ReturnsAsync(restaurants.Count);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _genericRepository.Verify(r => r.GetAllAsync(It.IsAny<GetAllRestaurantsSpecification>(), true), Times.Once);
        _genericRepository.Verify(r => r.CountAsync(null), Times.Once);

        result.Should().NotBeNull();
        result.Data.Should().HaveCount(restaurants.Count);
        result.PageIndex.Should().Be(query.PageNumber);
        result.PageSize.Should().Be(query.PageSize);
        result.Count.Should().Be(restaurants.Count);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyPagination_WhenNoRestaurants()
    {
        // Arrange
        var handler = CreateHandler();
        var query = CreateQuery();

        _genericRepository.Setup(r => r.GetAllAsync(It.IsAny<GetAllRestaurantsSpecification>(), true))
                          .ReturnsAsync(new List<RestaurantEntity>());

        _genericRepository.Setup(r => r.CountAsync(null))
                          .ReturnsAsync(0);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _genericRepository.Verify(r => r.GetAllAsync(It.IsAny<GetAllRestaurantsSpecification>(), true), Times.Once);
        _genericRepository.Verify(r => r.CountAsync(null), Times.Once);

        result.Should().NotBeNull();
        result.Data.Should().BeEmpty();
        result.PageIndex.Should().Be(query.PageNumber);
        result.PageSize.Should().Be(query.PageSize);
        result.Count.Should().Be(0);
    }
}
