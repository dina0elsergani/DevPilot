namespace DevPilot.Domain.ValueObjects;

public record Content
{
    public string Value { get; }

    private Content(string value)
    {
        Value = value;
    }

    public static Content Create(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));

        var trimmedContent = content.Trim();
        if (trimmedContent.Length > 500)
            throw new ArgumentException("Content cannot exceed 500 characters", nameof(content));

        return new Content(trimmedContent);
    }

    public static implicit operator string(Content content) => content.Value;

    public override string ToString() => Value;
} 