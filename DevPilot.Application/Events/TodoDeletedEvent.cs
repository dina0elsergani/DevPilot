using MediatR;

namespace DevPilot.Application.Events;

public class TodoDeletedEvent : INotification
{
    public int TodoId { get; set; }
    public int ProjectId { get; set; }
    public DateTime DeletedAt { get; set; }
} 