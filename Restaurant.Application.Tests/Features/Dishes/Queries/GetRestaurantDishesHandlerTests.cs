using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Restaurant.Application.Features.Dishes.Queries.GetRestaurantDishes;
using Restaurant.Application.Features.Dishes.Models.Responses;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Application.Common;
using Restaurant.Domain.Specifications.Dishes;

namespace Restaurant.Application.Test.Features.Dishes.Queries
{
    public class GetRestaurantDishesHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IGenericRepository<Dish, int>> _dishesRepositoryMock;
        private readonly GetRestaurantDishesQueryHandler _handler;

        public GetRestaurantDishesHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _dishesRepositoryMock = new Mock<IGenericRepository<Dish, int>>();

            _unitOfWorkMock
                .Setup(u => u.GetRepository<Dish, int>())
                .Returns(_dishesRepositoryMock.Object);

            _handler = new GetRestaurantDishesQueryHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_WhenDishesExist_ShouldReturnPaginatedDishes()
        {
            // Arrange
            var query = new GetRestaurantDishesQuery
            (
                RestaurantId : 1,
                PageNumber : 1,
                PageSize : 2,
                SearchText : null,
                SortBy : "Name",
                SortDirection : true
            );

            var dishes = new List<Dish>
            {
                new Dish { Id = 1, Name = "Pizza", RestaurantId = 1 },
                new Dish { Id = 2, Name = "Pasta", RestaurantId = 1 }
            };

            _dishesRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<GetRestaurantDishesSpecification>(), true))
                .ReturnsAsync(dishes);

            _dishesRepositoryMock
                .Setup(r => r.CountAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Dish, bool>>>()))
                .ReturnsAsync(dishes.Count);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(2, result.Data.Count());
            Assert.Contains(result.Data, d => d.Name == "Pizza");
        }
    }
}
