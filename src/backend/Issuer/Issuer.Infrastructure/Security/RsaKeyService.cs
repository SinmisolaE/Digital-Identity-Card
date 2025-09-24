using System;
using System.Security.Cryptography;
using Issuer.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Issuer.Infrastructure.Data;

public class RsaKeyService : IRsaKeyService
{
    private readonly RSA _rsa;

    public RsaKeyService()
    {
        _rsa = RSA.Create(2048);
        
    }
    public RsaSecurityKey PrivateKey()
    {
        return new RsaSecurityKey(_rsa);
    }

    public RsaSecurityKey PublicKey()
    {
        return new RsaSecurityKey(_rsa.ExportParameters(false));
        
    }

}
