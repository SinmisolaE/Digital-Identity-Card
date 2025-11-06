using System;
using System.Data.SqlTypes;
using TrustRegistryService.Core.DTO;
using TrustRegistryService.Core.Entity;
using TrustRegistryService.Core.Interfaces;

namespace TrustRegistryService.Core.Services;

public class RegistryService : IRegistryService
{
    private readonly IRegistryRepository _repository;
    //private readonly ILogger<RegistryService> _logger;

    public RegistryService(IRegistryRepository repository)//, ILogger<RegistryService> logger)
    {
        _repository = repository;
        //_logger = logger;

    }
    public async Task<RegistryDTO> GetRegistryByIssuerAsync(string issuer)
    {
        if (string.IsNullOrEmpty(issuer))
        {
            throw new ArgumentNullException("Issuer not provided");
        }

        var registry = await _repository.GetRegistryByIssuerAsync(issuer);

        if (registry == null)
        {
            throw new SqlNullValueException("Registry not found");
        }


        var registryDTO = new RegistryDTO(registry.IssuerId, registry.PublicKey, registry.Status.ToString());

        return registryDTO;
    }

    //Post registry logic: implemented by the issuer
    public async Task<bool> AddRegistryAsync(RegistryDTO registryDTO)
    {
        if (registryDTO == null)
        {
            throw new ArgumentNullException("Registry details not passed");
        }

        var registry = new TrustRegistry(registryDTO.IssuerId, registryDTO.PublicKey);

        if (registry == null)
        {
            throw new Exception("Cannot generate registry");
        }

        try
        {
            
            //check if issuer exists
            var existingIssuer = await _repository.GetRegistryByIssuerAsync(registry.IssuerId);

            System.Console.WriteLine($"Truing: {existingIssuer?.IssuerId}");
            if (existingIssuer != null)
            {
                throw new InvalidOperationException($"Issuer {existingIssuer} already exists!");
            }
            
            
            //Try to add to db, if issuerId exists, catch exception
            var response = await _repository.AddRegistryAsync(registry);

            if (response == null)
            {
                throw new Exception("Registry not added");
            }

            return true;

        }
        catch (Exception e)
        {
            throw new Exception($"Error Occured: {e.Message}");
        }


    }
}
