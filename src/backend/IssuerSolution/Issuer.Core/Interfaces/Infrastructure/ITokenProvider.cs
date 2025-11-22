using System;
using System.Security.Claims;
using Issuer.Core.Data;

namespace Issuer.Core.Interfaces.AuthService;

public interface ITokenProvider
{
    public string GeneratePasswordSetToken();

   // bool ValidatePasswordSetToken(string token);

    public string GenerateJwt(User user);

    public ClaimsPrincipal? ValidateJwtToken(string jwt);
}
