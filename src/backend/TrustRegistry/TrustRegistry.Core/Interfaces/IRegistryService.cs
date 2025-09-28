using System;
using TrustRegistryService.Core.DTO;
using TrustRegistryService.Core.Entity;

namespace TrustRegistryService.Core.Services;

public interface IRegistryService
{
    Task<RegistryDTO> GetRegistryByIssuer(string issuer);
}
