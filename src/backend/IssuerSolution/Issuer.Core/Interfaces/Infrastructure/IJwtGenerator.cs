using System;

namespace Issuer.Core.Interfaces;

public interface IJwtGenerator
{
    Task<string> GenerateJwtAsync(Citizen citizen);

}
