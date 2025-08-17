using Restaurant.Application.Features.Authentication.Commands.Register;
using Restaurant.Application.Features.Authentication.Commands.UpdateAccount;
using Restaurant.Application.Features.Authentication.Models.Responses;
using Restaurant.Domain.Entities;
using Restaurant.Infrastructure.Entities;

namespace Restaurant.Infrastructure.Mapping;

public static class IdentityMappingProfile
{
    public static ApplicationUser ToEntity(this UpdateAccountCommand updateAccountCommand, ApplicationUser existingEntity)
    {
        var entity = new ApplicationUser()
        {
            Id = existingEntity.Id,
            FullName = updateAccountCommand.FullName,
            UserName = updateAccountCommand.UserName,
            AccessFailedCount = existingEntity.AccessFailedCount,
            ConcurrencyStamp = existingEntity.ConcurrencyStamp,
            DateOfBirth = updateAccountCommand.DateOfBirth,
            Email = existingEntity.Email,
            EmailConfirmed = existingEntity.EmailConfirmed,
            LockoutEnd = existingEntity.LockoutEnd,
            LockoutEnabled = existingEntity.LockoutEnabled,
            PasswordHash = existingEntity.PasswordHash,
            SecurityStamp = existingEntity.SecurityStamp,
            NormalizedEmail = existingEntity.NormalizedEmail,
            NormalizedUserName = existingEntity.NormalizedUserName,
            PhoneNumber = updateAccountCommand.PhoneNumber,
            PhoneNumberConfirmed = existingEntity.PhoneNumberConfirmed,
            RefreshToken = existingEntity.RefreshToken,
            TwoFactorEnabled = existingEntity.TwoFactorEnabled
        };
        return entity;
    }
    public static ApplicationUser ToEntity(this RegisterCommand registerCommand)
    {
        var entity = new ApplicationUser()
        {
            Email = registerCommand.Email
        };
        return entity;
    }
    public static AccountResponse ToDto(this ApplicationUser applicationUser)
    {
        var dto = new AccountResponse(
            Id: applicationUser.Id,
            Email: applicationUser.Email!,
            UserName: applicationUser.UserName!, 
            DateOfBirth: applicationUser.DateOfBirth,
            FullName: applicationUser.FullName,
            PhoneNumber: applicationUser.PhoneNumber!
            );
        return dto;
    }
}