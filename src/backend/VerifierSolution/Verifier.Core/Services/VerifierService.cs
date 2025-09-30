using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Verifier.Core.DTO;
using Verifier.Core.Entity;
using Verifier.Core.Interfaces;

namespace Verifier.Core.Services;

public class VerifierService : IVerifierService
{
    private readonly ILogger<VerifierService> _logger;

    private readonly ITrustRegistryClient _trustRegistry;

    public VerifierService(ILogger<VerifierService> logger, ITrustRegistryClient trustRegistry)
    {
        _logger = logger;
        _trustRegistry = trustRegistry;
    }

    public async Task<CitizenDTO> GetCitizenAsync(JwtDTO jwtDTO)
    {
        if (jwtDTO == null || string.IsNullOrEmpty(jwtDTO.Jwt))
        {

            throw new Exception("Citizen details not passed");
        }

        var data = await _trustRegistry.GetRegistryByIssuerAsync("GRA");

        if (data == null)
        {
            throw new Exception("Issuer not found");
        }


        TrustRegistry registry = JsonSerializer.Deserialize<TrustRegistry>(data);

        if (registry.Status != TrustRegistry.SetStatus.Active)
        {
            throw new Exception("Issuer key is inactive! Contact Issuer for renewal");
        }



        if (validateJwt(jwtDTO.Jwt, registry.PublicKey))
        {
            
        }

        
    }
}
