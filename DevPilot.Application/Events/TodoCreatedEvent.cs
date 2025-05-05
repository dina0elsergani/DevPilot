using MediatR;

namespace DevPilot.Application.Events;

public class TodoCreatedEvent : INotification
{
    public int TodoId { get; set; }
    public int ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
} 