using System;

namespace Issuer.Core.Interfaces;

public interface IJwtGenerator
{
    string GenerateJwt(Citizen citizen);

}
