using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Authentication.Models.Responses;

namespace Restaurant.Application.Features.Authentication.Queries.GetCurrentUserAccount;

public class GetCurrentUserAccountQueryHandler(
    IAuthService authService,
    ILogger<GetCurrentUserAccountQueryHandler> logger
) : IRequestHandler<GetCurrentUserAccountQuery, AccountResponse>
{
    public async Task<AccountResponse> Handle(GetCurrentUserAccountQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetCurrentUserAccountQuery for user ID: {UserId}", request.Id);

        var account = await authService.GetAccountById(request.Id);

        return account;
    }
}