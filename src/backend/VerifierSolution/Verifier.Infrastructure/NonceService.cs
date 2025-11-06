using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Verifier.Core.DTO;
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

        _cache.Set(nonce, "pending", cacheEntryOptions);

        _logger.LogInformation($"Nonce generated: {nonce}");

        return nonce;
    }

    public bool MarkAsVerified(string nonce, CitizenDTO citizenData)
    {
        if (_cache.TryGetValue(nonce, out _))
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
                Size = 1
            };

            _logger.LogInformation("Setting cache");
            _cache.Set(nonce, citizenData, cacheEntryOptions); // Short expiry after verification

            _logger.LogInformation($"Nonce {nonce} marked as verified");
            return true;
        }

        _logger.LogWarning($"Nonce {nonce} not found or already used");
        return false;
    }
    
    public object GetStatus(string nonce)
    {
        if (_cache.TryGetValue(nonce, out object value))
        {
            if (value is string str && str == "pending")
                return new { status = "pending" };
            else if (value is CitizenDTO citizen)
                return new { status = "verified", citizenData = citizen };
        }
        
        return new { status = "expired" };
    }
    public bool IsValid(string nonce)
    {
        _logger.LogInformation($"trying to check validity: {nonce}");
        if (String.IsNullOrEmpty(nonce)) throw new Exception("No nonce provided");

        return _cache.TryGetValue(nonce, out object value) && value is string str && str == "pending";
    }

}
