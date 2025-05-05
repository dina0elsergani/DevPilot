using Microsoft.Extensions.Logging;

namespace DevPilot.Application.Services;

/// <summary>
/// Email service for sending notifications (example for background jobs).
/// </summary>
public class EmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public void SendTodoCompletionNotification(int todoId, string userEmail, string todoTitle)
    {
        _logger.LogInformation("Sending completion notification for todo {TodoId} to {UserEmail}", todoId, userEmail);
        
        // Simulate email sending
        Thread.Sleep(2000);
        
        _logger.LogInformation("Email sent successfully for todo {TodoId}", todoId);
    }

    public void SendWelcomeEmail(string userEmail, string userName)
    {
        _logger.LogInformation("Sending welcome email to {UserEmail} for user {UserName}", userEmail, userName);
        
        // Simulate email sending
        Thread.Sleep(1500);
        
        _logger.LogInformation("Welcome email sent successfully to {UserEmail}", userEmail);
    }

    public void SendDailyDigest(string userEmail, int completedTodos, int pendingTodos)
    {
        _logger.LogInformation("Sending daily digest to {UserEmail}: {CompletedTodos} completed, {PendingTodos} pending", 
            userEmail, completedTodos, pendingTodos);
        
        // Simulate email sending
        Thread.Sleep(3000);
        
        _logger.LogInformation("Daily digest sent successfully to {UserEmail}", userEmail);
    }
} 