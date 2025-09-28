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
    public async Task<RegistryDTO> GetRegistryByIssuer(string issuer)
    {
        if (string.IsNullOrEmpty(issuer))
        {
            throw new ArgumentNullException("Issuer not provided");
        }

        var registry = await _repository.GetRegistryByIssuer(issuer);

        if (registry == null)
        {
            throw new SqlNullValueException("Registry not found");
        }


        var registryDTO = new RegistryDTO(registry.IssuerId, registry.PublicKey, registry.Status.ToString());

        return registryDTO;
    }
}
