using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Restaurant.Application.Common;
using Restaurant.Application.Common.Messaging;
using Restaurant.Application.Features.Dishes.Models.Responses;

namespace Restaurant.Application.Features.Dishes.Queries.GetRestaurantDishes;


public sealed record GetRestaurantDishesQuery(
    int RestaurantId,
    string? SearchText,
    int PageSize,
    int PageNumber,
    string? SortBy,
    bool? SortDirection) :BasePaginatedQuery(
    SearchText,
    PageSize,
    PageNumber,
    SortBy,
    SortDirection
    ) , IRequest<Pagination<DishResponse>>;