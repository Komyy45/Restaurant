using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Common;
using Restaurant.Application.UseCases.Dishes.Commands.CreateRestaurantDish;
using Restaurant.Application.UseCases.Dishes.Commands.DeleteRestaurantDish;
using Restaurant.Application.UseCases.Dishes.Commands.UpdateRestaurantDish;
using Restaurant.Application.UseCases.Dishes.Dtos;
using Restaurant.Application.UseCases.Dishes.Queries.GetDishById;
using Restaurant.Application.UseCases.Dishes.Queries.GetRestaurantDishes;

namespace Restaurant.API.Controllers;

[Route("/api/Restaurants/{restaurantId:int}/[controller]")]
public class DishesController(IMediator mediator) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetRestaurantDishes([FromRoute] GetRestaurantDishesQuery request)
    {
        return Ok(await mediator.Send(request));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DishDto>> GetDishById([FromRoute] GetDishByIdQuery request)
    {
        return Ok(await mediator.Send(request));
    }

    [HttpPost]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateRestaurantDishCommand request)
    {
        request = request with { RestaurantId = restaurantId };
        var id = await mediator.Send(request);
        return CreatedAtAction(nameof(GetDishById), new { restaurantId, id }, null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDish([FromRoute] int id, [FromBody] UpdateRestaurantDishCommand request)
    {
        request = request with { Id = id };
        await mediator.Send(request);
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDish([FromRoute] DeleteRestaurantDishCommand request)
    {
        await mediator.Send(request);
        return NoContent();
    }
}