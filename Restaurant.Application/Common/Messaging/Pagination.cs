namespace Restaurant.Application.Common.Messaging;

public record Pagination<TPagination>(int PageIndex, int PageSize, int Count, IEnumerable<TPagination> Data);