using System;
using System.Net;
using System.Net.Http.Json;
using Verifier.Core.DTO;

using Verifier.Core.Interfaces;

namespace Verifier.Infrastructure;

public class TrustRegistryClient : ITrustRegistryClient
{
    private readonly ILogger<TrustRegistryClient> _logger;
    private readonly HttpClient _httpClient;

    public TrustRegistryClient(ILogger<TrustRegistryClient> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:5051");
    }

    // Get Issuer public key
    public async Task<string> GetRegistryByIssuerAsync(string issuer)
    {
        var response = await _httpClient.GetAsync($"/api/issuer/{issuer}");

        if (response == null)
        {
            throw new Exception("Issuer keys not available");
        }

        if (response.IsSuccessStatusCode)
        {
            string responseData = await response.Content.ReadAsStringAsync();
            return responseData;

        }
        throw new Exception("Issuer not found");

    }
}
