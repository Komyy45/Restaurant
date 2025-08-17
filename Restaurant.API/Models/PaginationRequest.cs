namespace Restaurant.API.Models;

public record PaginationRequest(
    string? SearchText,
    int PageSize,
    int PageNumber,
    string? SortBy,
    bool? SortDirection);