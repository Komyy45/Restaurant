using FluentAssertions;
using Restaurant.Application.Features.Restaurant.Commands.CreateRestaurant;
using Restaurant.Application.Features.Restaurant.Commands.UpdateRestaurant;
using Restaurant.Application.Mapping;
using Restaurant.Application.Features.Restaurant.Models.Responses;
using Restaurant.Domain.Entities;
using RestaurantEntity = Restaurant.Domain.Entities.Restaurant;

namespace Restaurant.Application.Test.Mapping;

public class RestaurantsMappingProfileTests
{
    [Fact]
    public void UpdateRestaurantCommandToEntity_WhenAddressIsNotNull_ThenMapsCorrectly()
    {
        var command = new UpdateRestaurantCommand(
            Id: 1,
            Name: "test",
            Description: "test",
            HasDelivery: false
        );

        var existingEntity = new RestaurantEntity
        {
            Id = 1,
            Name = "oldName",
            Description = "oldDescription",
            HasDelivery = true,
            ContactEmail = "test@gmail.com",
            ContactNumber = "12345",
            Address = new()
            {
                City = "oldCity",
                Street = "oldStreet",
                PostalCode = "00000"
            }
        };

        var result = Act(command, existingEntity);

        Assert(result, command, existingEntity);
    }
    
    [Fact]
    public void UpdateRestaurantCommandToEntity_WhenAddressIsNull_ThenMapsCorrectly()
    {
        var command = new UpdateRestaurantCommand(
            Id: 1,
            Name: "test",
            Description: "test",
            HasDelivery: false
        );

        var existingEntity = new RestaurantEntity
        {
            Id = 1
        };

        var result = Act(command, existingEntity);

        Assert(result, command, existingEntity);
    }
    
    [Fact]
    public void CreateRestaurantCommandToEntity_WhenAddressIsNotNull_ThenMapsCorrectly()
    {
        var command = new CreateRestaurantCommand(
            Name: "name",
            Description: "description",
            HasDelivery: false,
            ContactEmail: "contactEmail",
            ContactNumber: "contactNumber",
            Category: "category",
            PostalCode: "postalCode",
            City: "city",
            Street: "street"
        );

        string ownerId = "1";
        
        var result = Act(command, ownerId);
        
        Assert(result, command, ownerId);
    }
    
    [Fact]
    public void CreateRestaurantCommandToEntity_WhenAddressIsNull_ThenMapsCorrectly()
    {
        var command = new CreateRestaurantCommand(
            Name: null,
            Description: null,
            HasDelivery: false,
            ContactEmail: null,
            ContactNumber: null,
            Category: null!,
            PostalCode: null,
            City: null,
            Street: null
        );

        string ownerId = "1";
        
        var result = Act(command, ownerId);
        
        Assert(result, command, ownerId);
    }

    [Fact]
    public void RestaurantEntityToDto_WhenAddressIsNotNull_ThenMapsCorrectly()
    {
        var entity = new RestaurantEntity
        {
            Id = 1,
            Name = "test",
            Description = "testDesc",
            HasDelivery = true,
            Category = "category",
            ContactEmail = "test@gmail.com",
            ContactNumber = "12345",
            Address = new()
            {
                City = "city",
                Street = "street",
                PostalCode = "11111"
            }
        };

        var result = Act(entity);

        Assert(result, entity);
    }

    [Fact]
    public void RestaurantEntityToDto_WhenAddressIsNull_ThenMapsCorrectly()
    {
        var entity = new RestaurantEntity
        {
            Id = 1,
            Name = "test",
            Description = "testDesc",
            HasDelivery = true
        };

        var result = Act(entity);

        Assert(result, entity);
    }

    private RestaurantEntity Act(UpdateRestaurantCommand command, RestaurantEntity existingEntity)
        => command.ToEntity(existingEntity);

    private void Assert(RestaurantEntity result, UpdateRestaurantCommand command, RestaurantEntity existingEntity)
    {
        result.Should().NotBeNull();
        result.Id.Should().Be(existingEntity.Id);
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
        result.HasDelivery.Should().Be(command.HasDelivery);
        result.ContactEmail.Should().Be(existingEntity.ContactEmail);
        result.ContactNumber.Should().Be(existingEntity.ContactNumber);
        result.Address.Should().BeEquivalentTo(existingEntity.Address);
    }

    private RestaurantEntity Act(CreateRestaurantCommand command, string ownerId)
        => command.ToEntity(ownerId);
    
    private void Assert(RestaurantEntity result, CreateRestaurantCommand command, string ownerId)
    {
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
        result.HasDelivery.Should().Be(command.HasDelivery);
        result.ContactEmail.Should().Be(command.ContactEmail);
        result.ContactNumber.Should().Be(command.ContactNumber);
        result.Address.Should().BeEquivalentTo(new Address
        {
            City = command.City, 
            Street = command.Street, 
            PostalCode = command.PostalCode
        });
        result.OwnerId.Should().BeEquivalentTo(ownerId);
    }

    private RestaurantResponse Act(RestaurantEntity entity)
        => entity.ToDto();

    private void Assert(RestaurantResponse result, RestaurantEntity entity)
    {
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.Name.Should().Be(entity.Name);
        result.Description.Should().Be(entity.Description);
        result.HasDelivery.Should().Be(entity.HasDelivery);
        result.Category.Should().Be(entity.Category);
        result.ContactEmail.Should().Be(entity.ContactEmail);
        result.ContactNumber.Should().Be(entity.ContactNumber);
        result.City.Should().Be(entity.Address?.City);
        result.Street.Should().Be(entity.Address?.Street);
        result.Postalcode.Should().Be(entity.Address?.PostalCode);
    }
}
