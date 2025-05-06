using Microsoft.Extensions.Diagnostics.HealthChecks;
using DevPilot.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace DevPilot.Api.HealthChecks;

public class CustomHealthCheck : IHealthCheck
{
    private readonly ICacheService _cacheService;
    private readonly IBackgroundJobService _backgroundJobService;
    private readonly ILogger<CustomHealthCheck> _logger;

    public CustomHealthCheck(
        ICacheService cacheService,
        IBackgroundJobService backgroundJobService,
        ILogger<CustomHealthCheck> logger)
    {
        _cacheService = cacheService;
        _backgroundJobService = backgroundJobService;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var checks = new List<(string Name, Task<bool> Check)>();

            // Check cache service
            checks.Add(("Cache Service", CheckCacheServiceAsync()));

            // Check background job service
            checks.Add(("Background Job Service", CheckBackgroundJobServiceAsync()));

            // Wait for all checks to complete
            var results = await Task.WhenAll(checks.Select(async check =>
            {
                var isHealthy = await check.Check;
                return new { check.Name, IsHealthy = isHealthy };
            }));

            var unhealthyChecks = results.Where(r => !r.IsHealthy).ToList();

            if (unhealthyChecks.Any())
            {
                _logger.LogWarning("Health check failed for: {UnhealthyChecks}", 
                    string.Join(", ", unhealthyChecks.Select(c => c.Name)));
                
                return HealthCheckResult.Unhealthy(
                    "Some services are unhealthy",
                    data: unhealthyChecks.ToDictionary(c => c.Name, c => (object)c.IsHealthy));
            }

            _logger.LogInformation("All health checks passed");
            return HealthCheckResult.Healthy("All services are healthy");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed with exception");
            return HealthCheckResult.Unhealthy("Health check failed", ex);
        }
    }

    private async Task<bool> CheckCacheServiceAsync()
    {
        try
        {
            await _cacheService.SetAsync("health-check", "test", TimeSpan.FromSeconds(1));
            var result = await _cacheService.GetAsync<string>("health-check");
            return result == "test";
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> CheckBackgroundJobServiceAsync()
    {
        try
        {
            // Simple check to ensure background job service is responsive
            // In a real implementation, you might check if Hangfire dashboard is accessible
            return true; // Simplified for demo
        }
        catch
        {
            return false;
        }
    }
} 