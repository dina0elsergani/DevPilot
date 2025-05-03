using DevPilot.Domain.Events;

namespace DevPilot.Domain.Events;

public class TodoCreatedEvent : BaseDomainEvent
{
    public int TodoId { get; }
    public string Title { get; }
    public int ProjectId { get; }
    public string UserId { get; }

    public TodoCreatedEvent(int todoId, string title, int projectId, string userId)
    {
        TodoId = todoId;
        Title = title;
        ProjectId = projectId;
        UserId = userId;
    }

    public override string EventType => "TodoCreated";
}

public class TodoCompletedEvent : BaseDomainEvent
{
    public int TodoId { get; }
    public string UserId { get; }
    public DateTime CompletedAt { get; }

    public TodoCompletedEvent(int todoId, string userId)
    {
        TodoId = todoId;
        UserId = userId;
        CompletedAt = DateTime.UtcNow;
    }

    public override string EventType => "TodoCompleted";
}

public class TodoDeletedEvent : BaseDomainEvent
{
    public int TodoId { get; }
    public string UserId { get; }

    public TodoDeletedEvent(int todoId, string userId)
    {
        TodoId = todoId;
        UserId = userId;
    }

    public override string EventType => "TodoDeleted";
} 