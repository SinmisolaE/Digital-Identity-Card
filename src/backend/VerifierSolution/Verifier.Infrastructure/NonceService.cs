using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Verifier.Core.Interfaces;

namespace Verifier.Infrastructure;

public class NonceService : INonceService
{
    private readonly MemoryCache _cache;
    private readonly TimeSpan _defaultExpiry = TimeSpan.FromMinutes(5);
    private ILogger<NonceService> _logger;

    public NonceService(ILogger<NonceService> logger)
    {
        //cache options for memory cache
        var cacheOptions = new MemoryCacheOptions
        {
            SizeLimit = 1024,
            CompactionPercentage = 0.25,
            ExpirationScanFrequency = TimeSpan.FromMinutes(5)
        };
        _cache = new MemoryCache(cacheOptions);
        _logger = logger;
    }

    public string GenerateNonce()
    {
        _logger.LogInformation("Generating new nonce");

        //generating nonce
        var nonce = Guid.NewGuid().ToString("N");

        //cache options for storing nonce
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _defaultExpiry,
            Size = 1
        };

        _cache.Set(nonce, "active", cacheEntryOptions);

        _logger.LogInformation($"Nonce generated: {nonce}");

        return nonce;
    }

    public bool IsValid(string nonce)
    {
        if (String.IsNullOrEmpty(nonce)) throw new Exception("No nonce provided");

        // check if nonce is present
        if (_cache.TryGetValue(nonce, out _))
        {
            _logger.LogInformation($"Nonce found: {nonce}");

            _cache.Remove(nonce);


            return true;
        }

        _logger.LogWarning($"Nonce not right: {nonce}");

        return false;
    }

}
