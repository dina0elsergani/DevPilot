namespace DevPilot.Domain.ValueObjects;

public record Description
{
    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Description Create(string description)
    {
        if (description == null)
            return new Description(string.Empty);

        if (description.Length > 1000)
            throw new ArgumentException("Description cannot exceed 1000 characters", nameof(description));

        return new Description(description.Trim());
    }

    public static implicit operator string(Description description) => description.Value;

    public override string ToString() => Value;
} 