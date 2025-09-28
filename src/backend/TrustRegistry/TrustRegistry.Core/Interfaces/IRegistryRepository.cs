using System;
using TrustRegistryService.Core.Entity;

namespace TrustRegistryService.Core.Interfaces;

public interface IRegistryRepository
{
    Task<TrustRegistry?> GetRegistryByIssuer(string issuer);
}
