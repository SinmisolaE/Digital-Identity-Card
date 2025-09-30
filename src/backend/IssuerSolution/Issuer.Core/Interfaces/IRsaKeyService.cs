using System;
using Microsoft.IdentityModel.Tokens;

namespace Issuer.Core.Interfaces;

public interface IRsaKeyService
{/*
    RsaSecurityKey PrivateKey { get; }
    RsaSecurityKey PublicKey { get; }
    */
    RsaSecurityKey GetPrivateKey();

    string GetPublicKeyPem();
    string GetPrivateKeyPem();


}
