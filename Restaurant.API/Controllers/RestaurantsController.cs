using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Common;
using Restaurant.API.Models;
using Restaurant.Application.Common;
using Restaurant.Application.Common.Messaging;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Restaurant.Commands.CreateRestaurant;
using Restaurant.Application.Features.Restaurant.Commands.DeleteRestaurant;
using Restaurant.Application.Features.Restaurant.Commands.UpdateRestaurant;
using Restaurant.Application.Features.Restaurant.Models.Responses;
using Restaurant.Application.Features.Restaurant.Queries.GetAllRestaurants;
using Restaurant.Application.Features.Restaurant.Queries.GetRestaurantById;
using Restaurant.Domain.Common;

namespace Restaurant.API.Controllers;

public class RestaurantsController(IMediator mediator) : BaseApiController
{
    
    [HttpGet]
    public async Task<ActionResult<Pagination<RestaurantResponse>>> GetAll([FromQuery] PaginationRequest paginationRequest)
    {
        var request = new GetAllRestaurantsQuery(
            SearchText: paginationRequest.SearchText,
            PageSize: paginationRequest.PageSize,
            PageNumber: paginationRequest.PageNumber,
            SortBy: paginationRequest.SortBy,
            SortDirection: paginationRequest.SortDirection
        );
        var response = await mediator.Send(request);
        return Ok(response);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var request = new GetRestaurantByIdQuery(id);
        var response = await mediator.Send(request);
        return Ok(response);
    }
    
    [HttpPost]
    [Authorize(Roles = RoleTypes.Owner)]
    public async Task<ActionResult<int>> Create([FromBody] CreateRestaurantCommand request)
    {
        var id = await mediator.Send(request);
        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpPatch("{id:int}")]
    [Authorize(Roles = RoleTypes.Owner)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRestaurantCommand request)
    {
        request = request with { Id = id };
        await mediator.Send(request);
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = $"{RoleTypes.Owner},{RoleTypes.Admin}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var request = new DeleteRestaurantCommand(id);
        await mediator.Send(request);
        return NoContent();
    }
}