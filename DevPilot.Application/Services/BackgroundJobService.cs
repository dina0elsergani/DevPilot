using DevPilot.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Hangfire;
using System.Linq.Expressions;

namespace DevPilot.Application.Services;

/// <summary>
/// Background job service for enterprise-level job processing.
/// </summary>
public class BackgroundJobService : IBackgroundJobService
{
    private readonly ILogger<BackgroundJobService> _logger;

    public BackgroundJobService(ILogger<BackgroundJobService> logger)
    {
        _logger = logger;
    }

    public string Enqueue<T>(Expression<Action<T>> methodCall)
    {
        var jobId = BackgroundJob.Enqueue(methodCall);
        _logger.LogInformation("Enqueued background job {JobId} for {Method}", jobId, methodCall.ToString());
        return jobId;
    }

    public string Enqueue<T>(Expression<Action<T>> methodCall, TimeSpan delay)
    {
        var jobId = BackgroundJob.Schedule(methodCall, delay);
        _logger.LogInformation("Scheduled background job {JobId} for {Method} with delay {Delay}", 
            jobId, methodCall.ToString(), delay);
        return jobId;
    }

    public string Enqueue<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt)
    {
        var jobId = BackgroundJob.Schedule(methodCall, enqueueAt);
        _logger.LogInformation("Scheduled background job {JobId} for {Method} at {EnqueueAt}", 
            jobId, methodCall.ToString(), enqueueAt);
        return jobId;
    }

    public string EnqueueRecurring<T>(string recurringJobId, Expression<Action<T>> methodCall, string cronExpression)
    {
        RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression);
        _logger.LogInformation("Added/Updated recurring job {RecurringJobId} for {Method} with cron {Cron}", 
            recurringJobId, methodCall.ToString(), cronExpression);
        return recurringJobId;
    }

    public void DeleteRecurring(string recurringJobId)
    {
        RecurringJob.RemoveIfExists(recurringJobId);
        _logger.LogInformation("Deleted recurring job {RecurringJobId}", recurringJobId);
    }

    public bool Delete(string jobId)
    {
        var deleted = BackgroundJob.Delete(jobId);
        _logger.LogInformation("Deleted background job {JobId}: {Result}", jobId, deleted);
        return deleted;
    }
} 