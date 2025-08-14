using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Contracts;
using Restaurant.Application.UseCases.Authentication.Dtos;

namespace Restaurant.Application.UseCases.Authentication.Queries.GetCurrentUserAccount;

public class GetCurrentUserAccountQueryHandler(
    IAuthService authService,
    ILogger<GetCurrentUserAccountQueryHandler> logger
) : IRequestHandler<GetCurrentUserAccountQuery, AccountDto>
{
    public async Task<AccountDto> Handle(GetCurrentUserAccountQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetCurrentUserAccountQuery for user ID: {UserId}", request.Id);

        var account = await authService.GetAccountById(request.Id);

        return account;
    }
}