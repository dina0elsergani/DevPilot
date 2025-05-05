using MediatR;

namespace DevPilot.Application.Events;

public class TodoCompletedEvent : INotification
{
    public int TodoId { get; set; }
    public int ProjectId { get; set; }
    public DateTime CompletedAt { get; set; }
} 