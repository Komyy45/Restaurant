using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Restaurant.API.Controllers.Common;
using Restaurant.API.Models;
using Restaurant.Application.Common;
using Restaurant.Application.Common.Messaging;
using Restaurant.Application.Features.Dishes.Commands.CreateRestaurantDish;
using Restaurant.Application.Features.Dishes.Commands.DeleteRestaurantDish;
using Restaurant.Application.Features.Dishes.Commands.UpdateRestaurantDish;
using Restaurant.Application.Features.Dishes.Models.Responses;
using Restaurant.Application.Features.Dishes.Queries.GetDishById;
using Restaurant.Application.Features.Dishes.Queries.GetRestaurantDishes;
using Restaurant.Application.Features.Restaurant.Queries.GetRestaurantById;
using Restaurant.Domain.Common;

namespace Restaurant.API.Controllers;

[Route("/api/Restaurants/{restaurantId:int}/[controller]")]
public class DishesController(IMediator mediator) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<Pagination<DishResponse>>> GetRestaurantDishes([FromRoute] int restaurantId, [FromQuery] PaginationRequest paginationRequest)
    {
        var request = new GetRestaurantDishesQuery(
            RestaurantId: restaurantId,
            SearchText: paginationRequest.SearchText,
            PageNumber: paginationRequest.PageNumber,
            PageSize: paginationRequest.PageSize,
            SortBy: paginationRequest.SortBy,
            SortDirection: paginationRequest.SortDirection
            );
        return Ok(await mediator.Send(request));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DishResponse>> GetDishById([FromRoute] int restaurantId, [FromRoute] int id)
    {
        var request = new GetDishByIdQuery(restaurantId, id);
        return Ok(await mediator.Send(request));
    }

    [HttpPost]
    [Authorize(Roles = RoleTypes.Owner)]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateRestaurantDishCommand request)
    {
        request = request with { RestaurantId = restaurantId };
        var id = await mediator.Send(request);
        return CreatedAtAction(nameof(GetDishById), new { restaurantId, id }, null);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = RoleTypes.Owner)]
    public async Task<IActionResult> UpdateDish([FromRoute] int restaurantId, [FromRoute] int id, [FromBody] UpdateRestaurantDishCommand request)
    {
        request = request with { RestaurantId = restaurantId, Id = id };
        await mediator.Send(request);
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = $"{RoleTypes.Owner},{RoleTypes.Admin}")]
    public async Task<IActionResult> DeleteDish([FromRoute] int restaurantId, [FromRoute] int id)
    {
        var request = new DeleteRestaurantDishCommand(id, restaurantId);
        await mediator.Send(request);
        return NoContent();
    }
}