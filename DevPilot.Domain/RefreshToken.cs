using System;

namespace DevPilot.Domain;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Revoked { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;
    public bool IsExpired => DateTime.UtcNow >= Expires;
} 