using System;
using System.Security.Cryptography.X509Certificates;

using System.Text.Json;
using Microsoft.Extensions.Logging;
using Verifier.Core.DTO;
using Verifier.Core.Entity;
using Verifier.Core.Interfaces;

namespace Verifier.Core.Services;

public class VerifierService : IVerifierService
{
    private readonly ITrustRegistryClient _trustRegistry;

    private readonly ILogger<TrustRegistry> _logger;

    private readonly IJwtVerifier _jwtVerifier;

    public VerifierService(ITrustRegistryClient trustRegistry, IJwtVerifier jwtVerifier, ILogger<TrustRegistry> logger)
    {
        _logger = logger;
        _trustRegistry = trustRegistry;
        _jwtVerifier = jwtVerifier;
    }

    public async Task<CitizenDTO> GetCitizenAsync(JwtDTO jwtDTO)
    {
        _logger.LogInformation("Get citizen function service");
        if (jwtDTO == null || string.IsNullOrEmpty(jwtDTO.Jwt))
        {

            throw new Exception("Citizen details not passed");
        }

        _logger.LogInformation("Contacting trust registry");
        var data = await _trustRegistry.GetRegistryByIssuerAsync("gra");

        if (data == null)
        {
            _logger.LogWarning("Nothing exists");
            throw new Exception("Issuer not found");
        }

        _logger.LogInformation("data received");

        _logger.LogInformation("creating tr instance");

        System.Console.WriteLine($"Data:   {data.IssuerId}\n {data.PublicKey}\n {data.Status}");

        TrustRegistry registry = new TrustRegistry(data.IssuerId, data.PublicKey, (TrustRegistry.SetStatus)Enum.Parse(typeof(TrustRegistry.SetStatus), data.Status, ignoreCase: true));

        if (registry == null) throw new Exception("Issuer not found!");

        _logger.LogInformation("instance created 200");

        if (registry.Status != TrustRegistry.SetStatus.Active)
        {
            throw new Exception("Issuer key is inactive! Contact Issuer for renewal");
        }

        

        _logger.LogInformation("Get citizen from jwt");
        var citizen = _jwtVerifier.ValidateAndExtractCitizen(jwtDTO.Jwt, data.PublicKey);

        

        var citizenDTO = new CitizenDTO(citizen.FirstName, citizen.LastName, citizen.OtherNames,
            citizen.NationalIdNumber, citizen.Gender, citizen.DOB, citizen.PlaceOfBirth, citizen.Address,
            citizen.DateOfIssue, citizen.ExpiryDate, citizen.PublicKey
        );

        _logger.LogInformation("return citizen");
        return citizenDTO;
    }

    public string GetUrl()
    {
        _logger.LogInformation("Url to verify credentials which client comminucates to passed");
        return "http://localhost:5091/verifier/verify/";
    }

}
