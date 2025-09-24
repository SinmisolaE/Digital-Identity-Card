using System;
using Microsoft.IdentityModel.Tokens;

namespace Issuer.Core.Interfaces;

public interface IRsaKeyService
{
    RsaSecurityKey PrivateKey();
    RsaSecurityKey PublicKey();
}
