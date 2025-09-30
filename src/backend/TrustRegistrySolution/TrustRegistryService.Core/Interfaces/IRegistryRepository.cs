using System;
using TrustRegistryService.Core.Entity;

namespace TrustRegistryService.Core.Interfaces;

public interface IRegistryRepository
{
    Task<TrustRegistry?> GetRegistryByIssuerAsync(string issuer);

    Task<TrustRegistry?> AddRegistryAsync(TrustRegistry registry);
}
