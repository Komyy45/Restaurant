using Microsoft.AspNetCore.Identity;

namespace Restaurant.Identity.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public RefreshToken? RefreshToken { get; set; }
}