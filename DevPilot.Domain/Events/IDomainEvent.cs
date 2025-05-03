namespace DevPilot.Domain.Events;

/// <summary>
/// Base interface for all domain events.
/// </summary>
public interface IDomainEvent
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
    string EventType { get; }
} 