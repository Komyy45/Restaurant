namespace Restaurant.Application.Common.Messaging;

public abstract record BasePaginatedQuery(
    string? SearchText,
    int PageSize,
    int PageNumber,
    string? SortBy,
    bool? SortDirection);