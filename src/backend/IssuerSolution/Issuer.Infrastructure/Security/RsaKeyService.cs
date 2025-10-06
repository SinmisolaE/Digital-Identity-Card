using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Issuer.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace Issuer.Infrastructure.Data;

public class RsaKeyService : IRsaKeyService
{

    private readonly ITrustRegistryClient _trustRegistryClient;
    private readonly RSA rsa = RSA.Create(2048);
    private readonly ILogger<RsaKeyService> _logger;


    public RsaKeyService(ITrustRegistryClient trustRegistryClient, ILogger<RsaKeyService> logger)
    {

        _trustRegistryClient = trustRegistryClient;
        _logger = logger;
    }


    public RsaSecurityKey GetPrivateKey()
    {
        _logger.LogInformation("Trying to get private key");
        return new RsaSecurityKey(rsa.ExportParameters(true));
    }

    public string GetPrivateKeyPem()
    {
        _logger.LogInformation("Trying to get private key string");
        return rsa.ExportPkcs8PrivateKeyPem();
    }

    public async Task<string> GetPublicKeyPem()
    {
        _logger.LogInformation("Trying to get public key string");
        var publicKey = rsa.ExportSubjectPublicKeyInfoPem();

        _logger.LogInformation("Contacting registry");
        try
        {

            if (!await _trustRegistryClient.EnsureRegistered(publicKey))
            {
                _logger.LogWarning("Registry not found");
            }
        }
        catch (Exception e)
        {
            throw new Exception($"{e.Message}");
        }

        _logger.LogInformation("Returning publickey string");
        return publicKey;
    }

}
