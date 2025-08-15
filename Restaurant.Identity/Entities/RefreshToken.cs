namespace Restaurant.Infrastructure.Entities;

public sealed class RefreshToken
{
    public string UserId { get; set; } = default!;
    public string Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
}