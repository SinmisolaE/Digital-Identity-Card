using System;
using System.Data.Common;
using System.Net.Http.Json;
using Issuer.Core.DTO;
using Issuer.Core.Interfaces;
using Issuer.Core.Service;
using Issuer.Infrastructure.Data;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace Issuer.Infrastructure;

public class TrustRegistryClient : ITrustRegistryClient
{
    private readonly HttpClient _httpClient;
    private bool _isregistered = false;
    //private readonly object _lock = new object();

    // I added rsa here which caused circular reference error
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    private readonly ILogger<TrustRegistryClient> _logger;

    public TrustRegistryClient(HttpClient httpClient, ILogger<TrustRegistryClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> AddRegistryAsync(RegistryDTO registryDTO)
    {
        
        _logger.LogInformation("Trying to add registry");
        

        var response = await _httpClient.PostAsJsonAsync($"http://localhost:5051/register", registryDTO);

        if (response == null)
        {
            _logger.LogWarning("Response is null (Registry Not reachable)");
            throw new Exception("Registry Not reachable");
        }

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Registry added successfully");
            return true;
        }

        _logger.LogError("Registry not added");
        return false;
    }

    public async Task<bool> EnsureRegistered(string publicKey)
    {
        _logger.LogInformation("Ensuring if issuer is registered");
        System.Console.WriteLine($"Checker if key is registere: {_isregistered}");
        if (_isregistered) return true;

        /*
        lock (_lock)
        {
            if (_isregistered) return true;

            string publicKey = _rsa.GetPublicKeyPem();
            var registryDTO = new RegistryDTO("gra", publicKey, "Active");

            var response = await AddRegistryAsync(registryDTO);

            _isregistered = true;

            return true;
        }
        */

        await _semaphore.WaitAsync();

        try
        {
            if (_isregistered) return true;

            var registryDTO = new RegistryDTO("gra", publicKey, "Active");

            _logger.LogInformation("Issuer not added prior! Trying to add it now");
            var response = await AddRegistryAsync(registryDTO);

            _isregistered = response;

            return response;
        }
        catch (Exception e)
        {
            throw new Exception($"Error from db: {e.Message}");
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
