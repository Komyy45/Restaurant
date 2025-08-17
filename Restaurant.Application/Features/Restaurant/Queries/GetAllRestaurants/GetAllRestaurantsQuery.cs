using MediatR;
using Restaurant.Application.Common;
using Restaurant.Application.Common.Messaging;
using Restaurant.Application.Features.Restaurant.Models.Responses;

namespace Restaurant.Application.Features.Restaurant.Queries.GetAllRestaurants;

public sealed record GetAllRestaurantsQuery(
    string? SearchText,
    int PageSize,
    int PageNumber,
    string? SortBy,
    bool? SortDirection) : BasePaginatedQuery(
    SearchText,
    PageSize,
    PageNumber,
    SortBy,
    SortDirection
    ), IRequest<Pagination<RestaurantResponse>>;