using System.Text.RegularExpressions;

namespace DevPilot.Domain.ValueObjects;

public record UserId
{
    private static readonly Regex UserIdRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private UserId(string value)
    {
        Value = value;
    }

    public static UserId Create(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be empty", nameof(userId));

        if (!UserIdRegex.IsMatch(userId))
            throw new ArgumentException("Invalid user ID format (must be valid email)", nameof(userId));

        return new UserId(userId.ToLowerInvariant());
    }

    public static implicit operator string(UserId userId) => userId.Value;

    public override string ToString() => Value;
} 