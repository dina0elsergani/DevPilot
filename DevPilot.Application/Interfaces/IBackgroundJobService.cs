using System.Linq.Expressions;

namespace DevPilot.Application.Interfaces;

/// <summary>
/// Background job service interface for enterprise-level job processing.
/// </summary>
public interface IBackgroundJobService
{
    string Enqueue<T>(Expression<Action<T>> methodCall);
    string Enqueue<T>(Expression<Action<T>> methodCall, TimeSpan delay);
    string Enqueue<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt);
    string EnqueueRecurring<T>(string recurringJobId, Expression<Action<T>> methodCall, string cronExpression);
    void DeleteRecurring(string recurringJobId);
    bool Delete(string jobId);
} 