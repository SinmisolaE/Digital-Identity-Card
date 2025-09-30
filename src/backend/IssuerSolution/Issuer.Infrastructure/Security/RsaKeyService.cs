using System;
using System.Security.Cryptography;
using Issuer.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Issuer.Infrastructure.Data;

public class RsaKeyService : IRsaKeyService
{
    // private static readonly RSA rsa = RSA.Create(2048);

    private readonly RSA rsa = RSA.Create(2048);

    public RsaKeyService()
    {}


    public RsaSecurityKey GetPrivateKey()
    {
        return new RsaSecurityKey(rsa.ExportParameters(true));
    }

    public string GetPrivateKeyPem()
    {
        return rsa.ExportPkcs8PrivateKeyPem();
    }

    public string GetPublicKeyPem()
    {
        return rsa.ExportSubjectPublicKeyInfoPem();
    }

}
