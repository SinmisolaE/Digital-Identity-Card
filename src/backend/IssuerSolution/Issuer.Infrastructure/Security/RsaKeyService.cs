using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Issuer.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Configuration;
using Issuer.Core.DTO;

namespace Issuer.Infrastructure.Data;

/*
    Get private key
    generate key pairs
*/
public class RsaKeyService : IRsaKeyService
{

    private readonly ITrustRegistryClient _trustRegistryClient;
    //private readonly RSA rsa = RSA.Create(2048);
    private readonly ILogger<RsaKeyService> _logger;

    private readonly IConfiguration _configuration;


    public RsaKeyService(ITrustRegistryClient trustRegistryClient, ILogger<RsaKeyService> logger, IConfiguration configuration)
    {

        _trustRegistryClient = trustRegistryClient;
        _logger = logger;
        _configuration = configuration;
    }


    


    public async Task<RsaSecurityKey> GetPrivateKey()
    {
        _logger.LogInformation("Getting private key string");
        var privateKeyString = await GetPrivateKeyPem();

        // Replace literal \n with actual newlines (needed when reading from environment variables)
        privateKeyString = privateKeyString.Replace("\\n", "\n");

        RSA rsa = RSA.Create();

        _logger.LogInformation("Converting to key");
        rsa.ImportFromPem(privateKeyString);

        RsaSecurityKey privateKey = new RsaSecurityKey(rsa.ExportParameters(true));

        return privateKey;
    }

    public async Task<string> GetPrivateKeyPem()
    {
        _logger.LogInformation("Trying to get key from config");
        var privateKeyString = _configuration["RSA_PRIVATE_KEY"];

        _logger.LogInformation(privateKeyString);

        if (string.IsNullOrEmpty(privateKeyString))
        {
            _logger.LogInformation("Key not found... Generating");
            privateKeyString = await GenerateKeys();

            //add the private key string to configuration
        
            Console.WriteLine($"\n🚨 NEW RSA PRIVATE KEY GENERATED - ADD TO CONFIGURATION:\n");
            Console.WriteLine(privateKeyString);
            Console.WriteLine($"\nAdd this to your appsettings.Development.json or environment variables\n");
        
        }

        return privateKeyString;

        /*
        _logger.LogInformation("Trying to get private key string");
        return rsa.ExportPkcs8PrivateKeyPem();
        */
    }

    public async Task<string> GenerateKeys()
    {
        _logger.LogInformation("Into generate key function");
        RSA rsa = RSA.Create(2048);

        RsaSecurityKey privateKey = new RsaSecurityKey(rsa.ExportParameters(true));

        var privateKeyString = rsa.ExportPkcs8PrivateKeyPem();


        var publicKey = rsa.ExportSubjectPublicKeyInfoPem();

        System.Console.WriteLine($"Private: {privateKey}");
        System.Console.WriteLine();
        System.Console.WriteLine();

        System.Console.WriteLine($"Public: {publicKey}");

        try
        {
            _logger.LogInformation("Adding public key to registry");
            var registryDTO = new RegistryDTO("gra", publicKey, "Active");

            var resp = await _trustRegistryClient.AddRegistryAsync(registryDTO);

            if (resp)
            {
                _logger.LogInformation("Public key added");
                return privateKeyString;
                
            } else
            {
                _logger.LogInformation("Public key not added");
                throw new Exception("PUblic key not stored");
            }

        }
        catch (Exception e)
        {
            throw new Exception($"Error from db: {e.Message}");
        }

        
    }
    
    /*

    public async Task<string> GetPublicKeyPem()
    {
        var registryDTO = new RegistryDTO("gra", publicKey, "Active");




        /*
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
        */

}
