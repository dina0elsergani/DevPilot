namespace DevPilot.Domain.ValueObjects;

public record Title
{
    public string Value { get; }

    private Title(string value)
    {
        Value = value;
    }

    public static Title Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        var trimmedTitle = title.Trim();
        if (trimmedTitle.Length > 200)
            throw new ArgumentException("Title cannot exceed 200 characters", nameof(title));

        return new Title(trimmedTitle);
    }

    public static implicit operator string(Title title) => title.Value;

    public override string ToString() => Value;
} 