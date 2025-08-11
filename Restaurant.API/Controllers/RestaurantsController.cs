using System.Runtime.InteropServices.JavaScript;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Common;
using Restaurant.Application.Models.Restaurants;
using Restaurant.Application.UseCases.Restaurant.Commands.CreateRestaurant;
using Restaurant.Application.UseCases.Restaurant.Queries.GetAllRestaurants;
using Restaurant.Application.UseCases.Restaurant.Queries.GetRestaurantById;

namespace Restaurant.API.Controllers;

public class RestaurantsController(IMediator mediator) : BaseApiController
{
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
    {
        var request = new GetAllRestaurantsQuery();
        var response = await mediator.Send(request);
        return Ok(response);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RestaurantDto>> Get([FromRoute] int id)
    {
        var request = new GetRestaurantByIdQuery(id);
        var response = await mediator.Send(request);
        return response is null ? NotFound() : Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateRestaurantCommand createRestaurantCommand)
    {
        var id = await mediator.Send(createRestaurantCommand);
        return Ok(id);
    }
}