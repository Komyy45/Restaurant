using Moq;
using Restaurant.Application.Features.Restaurant.Queries.GetRestaurantById;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Test.Features.Restaurants.Queries
{
    public class GetRestaurantByIdHandlerTests
    {
        private readonly Mock<IGenericRepository<Domain.Entities.Restaurant, int>> _restaurantRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetRestaurantByIdQueryHandler _handler;

        public GetRestaurantByIdHandlerTests()
        {
            _restaurantRepositoryMock = new Mock<IGenericRepository<Domain.Entities.Restaurant, int>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock
                .Setup(u => u.GetRepository<Domain.Entities.Restaurant, int>())
                .Returns(_restaurantRepositoryMock.Object);

            _handler = new GetRestaurantByIdQueryHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_WhenRestaurantExists_ShouldReturnRestaurantResponse()
        {
            var restaurant = new Domain.Entities.Restaurant
            {
                Id = 1,
                Name = "Test Restaurant",
                Description = "Test Description",
                HasDelivery = true,
                ContactEmail = "test@email.com",
                ContactNumber = "1234567890"
            };

            _restaurantRepositoryMock
                .Setup(r => r.GetAsync(1))
                .ReturnsAsync(restaurant);

            var query = new GetRestaurantByIdQuery(1);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(restaurant.Id, result.Id);
            Assert.Equal(restaurant.Name, result.Name);
            Assert.Equal(restaurant.Description, result.Description);
            Assert.Equal(restaurant.HasDelivery, result.HasDelivery);
            Assert.Equal(restaurant.ContactEmail, result.ContactEmail);
            Assert.Equal(restaurant.ContactNumber, result.ContactNumber);
        }

        [Fact]
        public async Task Handle_WhenRestaurantDoesNotExist_ShouldThrowNotFoundException()
        {
            _restaurantRepositoryMock
                .Setup(r => r.GetAsync(2))
                .ReturnsAsync((Domain.Entities.Restaurant)null);

            var query = new GetRestaurantByIdQuery(2);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}