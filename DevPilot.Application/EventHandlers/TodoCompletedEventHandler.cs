using MediatR;
using DevPilot.Application.Events;
using Microsoft.Extensions.Logging;

namespace DevPilot.Application.EventHandlers;

public class TodoCompletedEventHandler : INotificationHandler<TodoCompletedEvent>
{
    private readonly ILogger<TodoCompletedEventHandler> _logger;

    public TodoCompletedEventHandler(ILogger<TodoCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(TodoCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Todo {TodoId} was completed at {CompletedAt}", 
            notification.TodoId, notification.CompletedAt);

        // In a real application, this could trigger:
        // - Email notifications
        // - Slack/Discord notifications
        // - Analytics tracking
        // - Achievement unlocks
        // - Project progress updates
        // - Background job processing

        await Task.CompletedTask; // Simulate async work
    }
} 