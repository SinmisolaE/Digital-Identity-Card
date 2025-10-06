using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Verifier.Core.DTO;

using Verifier.Core.Interfaces;

namespace Verifier.Infrastructure;

public class TrustRegistryClient : ITrustRegistryClient
{
    private readonly ILogger<TrustRegistryClient> _logger;
    private readonly HttpClient _httpClient;

    public TrustRegistryClient(HttpClient httpClient, ILogger<TrustRegistryClient> logger)
    {
        _logger = logger;
        _httpClient = httpClient;
        //_httpClient.BaseAddress = new Uri("http://localhost:5051/");
    }

    // Get Issuer public key
    public async Task<RegistryDTO> GetRegistryByIssuerAsync(string issuer)
    {
        _logger.LogInformation("Sending request");

        var url = $"/issuer?issuer={issuer}";
        _logger.LogInformation("🔍 Calling URL: {Url}", url);


        var response = await _httpClient.GetAsync("http://localhost:5051/issuer?issuer=gra");

        _logger.LogInformation($"Status: {response.StatusCode}");

        if (response == null)
        {
            _logger.LogWarning("Response is null");
            throw new Exception("Issuer keys not available");
        }

        if (response.IsSuccessStatusCode)
        {
            try
            {
                _logger.LogInformation("Request successful");
                
                // Use ReadFromJsonAsync for better error handling
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                var responseDTO = await response.Content.ReadFromJsonAsync<RegistryDTO>(options);
                
                if (responseDTO == null)
                {
                    throw new Exception("Registry returned empty response");
                }
                
                if (string.IsNullOrEmpty(responseDTO.PublicKey))
                {
                    _logger.LogWarning(" PublicKey is null or empty in response");
                    throw new Exception("Public key not found in registry response");
                }
                
                _logger.LogInformation("Successfully retrieved public key for issuer");
                return responseDTO;
            }
            catch (Exception ex) when (ex is JsonException or NotSupportedException)
            {
                _logger.LogError(ex, " Failed to parse registry response");
                throw new Exception("Registry service returned invalid data format");
            }
        }

        /*

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Request successfull");
            string responseData = await response.Content.ReadAsStringAsync();
            var responseDTO = JsonSerializer.Deserialize<RegistryDTO>(responseData);
            if (responseDTO == null) throw new Exception("Key not found");
            _logger.LogInformation("returning response")
            return responseDTO;

        }

        */
        throw new Exception("Issuer not found");

    }
}
