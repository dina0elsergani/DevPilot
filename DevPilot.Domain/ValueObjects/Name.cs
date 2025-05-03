namespace DevPilot.Domain.ValueObjects;

public record Name
{
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Name Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Name cannot exceed 100 characters", nameof(name));

        return new Name(name.Trim());
    }

    public static implicit operator string(Name name) => name.Value;

    public override string ToString() => Value;
} 