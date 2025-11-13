using System;

namespace Verifier.Core.Interfaces;

public interface ICacheService
{
    Task<string?> GetStringAsync(string cacheKey);

    Task<bool> SetStringAsync(string cacheKey, string publicKey);
}
