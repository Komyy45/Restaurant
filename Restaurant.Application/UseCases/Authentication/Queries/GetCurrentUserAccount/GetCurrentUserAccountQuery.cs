using MediatR;
using Restaurant.Application.UseCases.Authentication.Dtos;

namespace Restaurant.Application.UseCases.Authentication.Queries.GetCurrentUserAccount;

public sealed record GetCurrentUserAccountQuery(string Id) : IRequest<AccountDto>; 