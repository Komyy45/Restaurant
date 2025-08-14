using Microsoft.AspNetCore.Identity;

namespace Restaurant.Persistence.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public DateOnly DateOfBirth { get; set; }
}