using System;
using Verifier.Core.DTO;

namespace Verifier.Core.Interfaces;

public interface ITrustRegistryClient
{
    Task<string> GetRegistryByIssuerAsync(string issuer);

}
