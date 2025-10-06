using System;
using Verifier.Core.Enitity;

namespace Verifier.Core.Interfaces;

public interface IJwtVerifier
{
    Citizen ValidateAndExtractCitizen(string jwt, string publicKeyPem);

}
