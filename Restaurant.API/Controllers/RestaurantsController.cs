using System.Runtime.InteropServices.JavaScript;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Common;
using Restaurant.Application.Features.Restaurant.Commands.CreateRestaurant;
using Restaurant.Application.Features.Restaurant.Commands.DeleteRestaurant;
using Restaurant.Application.Features.Restaurant.Commands.UpdateRestaurant;
using Restaurant.Application.Features.Restaurant.Models.Responses;
using Restaurant.Application.Features.Restaurant.Queries.GetAllRestaurants;
using Restaurant.Application.Features.Restaurant.Queries.GetRestaurantById;

namespace Restaurant.API.Controllers;

public class RestaurantsController(IMediator mediator) : BaseApiController
{
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantResponse>>> GetAll()
    {
        var request = new GetAllRestaurantsQuery();
        var response = await mediator.Send(request);
        return Ok(response);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RestaurantResponse>> Get([FromRoute] int id)
    {
        var request = new GetRestaurantByIdQuery(id);
        var response = await mediator.Send(request);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateRestaurantCommand createRestaurantCommand)
    {
        var id = await mediator.Send(createRestaurantCommand);
        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRestaurantCommand request)
    {
        request = request with { Id = id };
        await mediator.Send(request);
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var request = new DeleteRestaurantCommand(id);
        await mediator.Send(request);
        return Ok();
    }
}