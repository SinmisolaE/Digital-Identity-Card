using System;
using Issuer.Core.DTO;
using Microsoft.Win32;

namespace Issuer.Core.Interfaces;

public interface ITrustRegistryClient
{
    Task<bool> AddRegistryAsync(RegistryDTO registryDTO);

    Task<bool> EnsureRegistered(string publicKey);

}
