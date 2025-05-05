using MediatR;

namespace DevPilot.Application.Events;

public class TodoUncompletedEvent : INotification
{
    public int TodoId { get; set; }
    public int ProjectId { get; set; }
    public DateTime UncompletedAt { get; set; }
} 