using MediatR;
using Restaurant.Application.Features.Authentication.Models.Responses;

namespace Restaurant.Application.Features.Authentication.Queries.GetCurrentUserAccount;

public sealed record GetCurrentUserAccountQuery(string Id) : IRequest<AccountResponse>; 