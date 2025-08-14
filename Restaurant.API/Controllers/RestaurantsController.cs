using System.Runtime.InteropServices.JavaScript;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Common;
using Restaurant.Application.UseCases.Restaurant.Commands.CreateRestaurant;
using Restaurant.Application.UseCases.Restaurant.Commands.DeleteRestaurant;
using Restaurant.Application.UseCases.Restaurant.Commands.UpdateRestaurant;
using Restaurant.Application.UseCases.Restaurant.Dtos;
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
    public async Task<ActionResult<RestaurantDto>> Get([FromRoute] GetRestaurantByIdQuery request)
    {
        var response = await mediator.Send(request);
        return response is null ? NotFound() : Ok(response);
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateRestaurantCommand createRestaurantCommand)
    {
        var id = await mediator.Send(createRestaurantCommand);
        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<bool>> Update([FromRoute] int id, [FromBody] UpdateRestaurantCommand request)
    {
        request = request with { Id = id };
        var response = await mediator.Send(request);
        return Ok(response);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] DeleteRestaurantCommand request)
    {
        await mediator.Send(request);
        return Ok();
    }
}