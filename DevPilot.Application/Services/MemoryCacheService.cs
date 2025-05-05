using DevPilot.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace DevPilot.Application.Services;

/// <summary>
/// Memory cache service implementation for enterprise-level caching.
/// </summary>
public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<MemoryCacheService> _logger;

    public MemoryCacheService(IMemoryCache memoryCache, ILogger<MemoryCacheService> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public Task<T?> GetAsync<T>(string key)
    {
        var value = _memoryCache.Get<T>(key);
        _logger.LogDebug("Cache {Action} for key {Key}: {Result}", "GET", key, value != null ? "HIT" : "MISS");
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new MemoryCacheEntryOptions();
        if (expiration.HasValue)
        {
            options.AbsoluteExpirationRelativeToNow = expiration;
        }

        _memoryCache.Set(key, value, options);
        _logger.LogDebug("Cache {Action} for key {Key} with expiration {Expiration}", "SET", key, expiration);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _memoryCache.Remove(key);
        _logger.LogDebug("Cache {Action} for key {Key}", "REMOVE", key);
        return Task.CompletedTask;
    }

    public Task RemoveByPatternAsync(string pattern)
    {
        // Note: MemoryCache doesn't support pattern-based removal natively
        // This is a simplified implementation. In production, consider using Redis or a more sophisticated cache
        _logger.LogWarning("Pattern-based cache removal not fully supported in MemoryCache. Pattern: {Pattern}", pattern);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string key)
    {
        var exists = _memoryCache.TryGetValue(key, out _);
        return Task.FromResult(exists);
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        var cachedValue = await GetAsync<T>(key);
        if (cachedValue != null)
        {
            return cachedValue;
        }

        var value = await factory();
        await SetAsync(key, value, expiration);
        return value;
    }
} 