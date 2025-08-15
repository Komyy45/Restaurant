using Mapster;
using Restaurant.Application.Features.Authentication.Commands.Register;
using Restaurant.Application.Features.Authentication.Commands.UpdateAccount;
using Restaurant.Application.Features.Authentication.Models.Responses;
using Restaurant.Infrastructure.Entities;

namespace Restaurant.Infrastructure.Mapping;

public static class IdentityMappingProfile
{
    public static ApplicationUser ToEntity(this UpdateAccountCommand updateAccountCommand, ApplicationUser user) => updateAccountCommand.Adapt(user);
    public static ApplicationUser ToEntity(this RegisterCommand registerCommand) => registerCommand.Adapt<ApplicationUser>();
    public static AccountResponse ToDto(this ApplicationUser applicationUser) => applicationUser.Adapt<AccountResponse>();
}