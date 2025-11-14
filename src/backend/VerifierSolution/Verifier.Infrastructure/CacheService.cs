using System;
using Verifier.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Verifier.Infrastructure;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IDistributedCache distributedCache, ILogger<CacheService> logger)
    {
        _distributedCache = distributedCache;
        _logger = logger;
        
    }
    public async Task<string?> GetStringAsync(string cacheKey)
    {
        var response = "";
        try
        {
            _logger.LogInformation("Cheching for public key in cache");
            response = await _distributedCache.GetStringAsync(cacheKey);
        } catch (Exception e)
        {
            _logger.LogError($"Not reading from cache: {e.Message}");
            return null;  
        }

        if (string.IsNullOrEmpty(response))
        {
            _logger.LogInformation($"Public key for {cacheKey} not found in cache");
            return null;
        }

        _logger.LogInformation("key in cache");
        return response;
    }

    public async Task<bool> SetStringAsync(string cacheKey, string publicKey)
    {
        _logger.LogInformation($"Adding key for {cacheKey} to cache");

        try
        {

            await _distributedCache.SetStringAsync(cacheKey, publicKey, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(10)
            });

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError($"String not set to cache: {e.Message}");
            return false;
        }
    }
}
