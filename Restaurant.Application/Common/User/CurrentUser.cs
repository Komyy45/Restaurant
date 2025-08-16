namespace Restaurant.Application.Common.User;

public sealed class CurrentUser
{
    public string Id { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string UserName { get; init; } = default!;
    public IEnumerable<string> Roles { get; init; } = default!;
    
    public bool IsInRole(string role) => Roles.Contains(role);
}