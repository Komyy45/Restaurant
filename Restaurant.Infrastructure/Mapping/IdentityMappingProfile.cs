using Mapster;
using Restaurant.Application.UseCases.Authentication.Commands.RegisterCommand;
using Restaurant.Application.UseCases.Authentication.Commands.UpdateAccount;
using Restaurant.Application.UseCases.Authentication.Dtos;
using Restaurant.Persistence.Identity;

namespace Restaurant.Infrastructure.Mapping;

public static class IdentityMappingProfile
{
    public static ApplicationUser ToEntity(this UpdateAccountCommand updateAccountCommand, ApplicationUser user) => updateAccountCommand.Adapt(user);
    public static ApplicationUser ToEntity(this RegisterCommand registerCommand) => registerCommand.Adapt<ApplicationUser>();
    public static AccountDto ToDto(this ApplicationUser applicationUser) => applicationUser.Adapt<AccountDto>();
}