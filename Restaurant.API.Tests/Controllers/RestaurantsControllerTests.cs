using System.Net;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Restaurant.API.Tests.Controllers.Common;
using Restaurant.Application.Features.Restaurant.Commands.CreateRestaurant;
using Restaurant.Application.Features.Restaurant.Commands.UpdateRestaurant;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Restaurant.API.Tests.Controllers;
using RestaurantEntity = Domain.Entities.Restaurant;

public class RestaurantsControllerTests(WebApplicationFactory<Program> webApplicationFactory) : BaseControllerTests<RestaurantEntity, int>(webApplicationFactory)
{
    private readonly string _ownerId = "1";
    
    [Fact]
    public async Task GetAll_ForValidRequest_ShouldReturn200Ok()
    {
        // arrange
        var client = webApplicationFactory.CreateClient();
        
        // act
       var response = await client.GetAsync("api/Restaurants?PageNumber=1&PageSize=10");

       // assert
       response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetAll_ForNonValidRequest_ShouldReturn400BadRequest()
    {
        // arrange
        var client = webApplicationFactory.CreateClient();

        // act
        var response = await client.GetAsync("api/Restaurants");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetById_ForValidRequest_ShouldReturn200Ok()
    {
        // arrange
        var client = NonAuthorizedPolicyEvaluator().CreateClient();
        var restaurant = new RestaurantEntity(){ Id = 1 };
        _repositoryMock.Setup(  r => r.GetAsync(1))
            .Returns(() => Task.FromResult(restaurant)!);
       
        // act
        var response = await client.GetAsync($"api/Restaurants/1");

        // assert
       response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
   [Fact]
    public async Task GetById_ForNonValidRequest_ShouldReturn400BadRequest()
    {
        // arrange
        var client = webApplicationFactory.CreateClient();
        
        // act
       var response = await client.GetAsync($"api/Restaurants/-1");

       // assert
       response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetById_ForRestaurantThatDoesntExist_ShouldReturn404NotFound()
    { 
        // arrange
        var client = NonAuthorizedPolicyEvaluator().CreateClient();
        _repositoryMock.Setup(  r => r.GetAsync(1000))
            .Returns(() => Task.FromResult<RestaurantEntity>(null!)!);
        
        // act
        var response = await client.GetAsync($"api/Restaurants/1000");
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateRestaurant_ForValidRequest_ShouldReturn201Created()
    {
        // arrange
        var factory = OwnerPolicyEvaluator(_ownerId);
        _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<RestaurantEntity>()))
            .Returns(() => ValueTask.CompletedTask);
        
        var restaurant = new CreateRestaurantCommand(
            Name: "Test Restaurant",
            Description: "Test Description",
            Category: "Italian",
            HasDelivery: true,
            ContactEmail: "test@test.com",
            ContactNumber: "123456789",
            City: "Test City",
            Street: "Test Street",
            PostalCode: "12-345"
        );
        var body = JsonSerializer.Serialize(restaurant);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var client = factory.CreateClient();

        // act
        var response = await client.PostAsync("api/restaurants", content);
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    
    [Fact]
    public async Task CreateRestaurant_ForValidRequestWithNonOwnerRole_ShouldReturn403Forbidden()
    {
        // arrange
        var factory = CustomerPolicyEvaluator();
        
        var restaurant = new CreateRestaurantCommand(
            Name: "Test Restaurant",
            Description: "Test Description",
            Category: "Italian",
            HasDelivery: true,
            ContactEmail: "test@test.com",
            ContactNumber: "123456789",
            City: "Test City",
            Street: "Test Street",
            PostalCode: "12-345"
        );
        var body = JsonSerializer.Serialize(restaurant);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var client = factory.CreateClient();

        // act
        var response = await client.PostAsync("api/restaurants", content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task CreateRestaurant_ForNonValidRequest_ShouldReturn400BadRequest()
    {
        // arrange
        var factory = OwnerPolicyEvaluator(_ownerId);

        var restaurant = new { };
        var body = JsonSerializer.Serialize(restaurant);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var client = factory.CreateClient();

        // act
        var response = await client.PostAsync("api/restaurants", content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    
    [Fact]
public async Task Update_ForValidRequest_ShouldReturn204NoContent()
{
    // arrange
    var client = OwnerPolicyEvaluator(_ownerId).CreateClient();
    var restaurant = new RestaurantEntity() { Id = 1000, OwnerId = _ownerId };

    _repositoryMock.Setup(r => r.GetAsync(1000))
        .Returns(() => Task.FromResult(restaurant)!);

    _repositoryMock.Setup(r => r.Update(It.IsAny<RestaurantEntity>()));

    var updateRequest = new UpdateRestaurantCommand(
        Id: 1000,
        Name: "Updated Name",
        Description: "Updated Description",
        HasDelivery: false
    );

    var body = JsonSerializer.Serialize(updateRequest);
    var content = new StringContent(body, Encoding.UTF8, "application/json");

    // act
    var response = await client.PatchAsync("api/restaurants/1000", content);

    // assert
    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
}

[Fact]
public async Task Update_ForNonOwnerRole_ShouldReturn403Forbidden()
{
    // arrange
    var client = CustomerPolicyEvaluator().CreateClient();
    _repositoryMock.Setup(r => r.GetAsync(1000))
        .Returns(() => Task.FromResult<RestaurantEntity>(new RestaurantEntity(){ Id = 1000 })!);

    var updateRequest = new UpdateRestaurantCommand(
        Id: 1000,
        Name: "Updated Name",
        Description: "Updated Description",
        HasDelivery: false
    );
    
    var body = JsonSerializer.Serialize(updateRequest);
    var content = new StringContent(body, Encoding.UTF8, "application/json");

    // act
    var response = await client.PatchAsync("api/restaurants/1000", content);

    // assert
    response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
}

[Fact]
public async Task Update_ForNonValidRequest_ShouldReturn400BadRequest()
{
    // arrange
    var client = OwnerPolicyEvaluator(_ownerId).CreateClient();

    var updateRequest = new { }; // empty invalid body
    var body = JsonSerializer.Serialize(updateRequest);
    var content = new StringContent(body, Encoding.UTF8, "application/json");

    // act
    var response = await client.PatchAsync("api/restaurants/1000", content);

    // assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
}

[Fact]
public async Task Update_ForRestaurantThatDoesntExist_ShouldReturn404NotFound()
{
    // arrange
    var client = OwnerPolicyEvaluator(_ownerId).CreateClient();

    _repositoryMock.Setup(r => r.GetAsync(1000))
        .Returns(() => Task.FromResult<RestaurantEntity>(null!)!);

    var updateRequest = new UpdateRestaurantCommand(
        Id: 1000,
        Name: "Updated Name",
        Description: "Updated Description",
        HasDelivery: false
    );
    var body = JsonSerializer.Serialize(updateRequest);
    var content = new StringContent(body, Encoding.UTF8, "application/json");

    // act
    var response = await client.PatchAsync("api/restaurants/1000", content);

    // assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
}
    
    
    [Fact]
    public async Task Delete_ForValidRestaurant_ShouldReturn204NoContent()
    { 
        // arrange
        var restaurant = new RestaurantEntity(){ OwnerId = _ownerId };
        var client = OwnerPolicyEvaluator(_ownerId).CreateClient();
        _repositoryMock.Setup(  r => r.GetAsync(1000))
            .Returns(() => Task.FromResult(restaurant)!);
        _repositoryMock.Setup(r => r.Delete(restaurant));
        
        // act
        var response = await client.DeleteAsync($"api/Restaurants/1000");
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Delete_ForNonAuthorizedUsers_ShouldReturn403Forbidden()
    { 
        // arrange
        var restaurant = new RestaurantEntity(){ OwnerId = "NON_MATCHING_ID" };
        var client = OwnerPolicyEvaluator(_ownerId).CreateClient();
        _repositoryMock.Setup(  r => r.GetAsync(1000))
            .Returns(() => Task.FromResult(restaurant)!);
        _repositoryMock.Setup(r => r.Delete(restaurant));
        
        // act
        var response = await client.DeleteAsync($"api/Restaurants/1000");
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Delete_ForRestaurantThatDoesntExist_ShouldReturn404NotFound()
    { 
        // arrange
        var client = OwnerPolicyEvaluator(_ownerId).CreateClient();
        _repositoryMock.Setup(  r => r.GetAsync(1000))
            .Returns(() => Task.FromResult<RestaurantEntity>(null!)!);
        
        // act
        var response = await client.DeleteAsync($"api/Restaurants/1000");
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}