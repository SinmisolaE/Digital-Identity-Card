using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Issuer.Core.Data;
using Issuer.Core.Interfaces.AuthService;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Issuer.Infrastructure.Security;

public class UserTokenProvider : ITokenProvider
{
    private readonly IConfiguration _configuration;

    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationToken;

    public UserTokenProvider(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = _configuration["Jwt:Secret"];
        _issuer = _configuration["Jwt:Issuer"];
        _audience = _configuration["Jwt:Audience"];
        _expirationToken = _configuration.GetValue<int>("Jwt:ExpirationToken");
        
    }

    // Generate jwt token upon user login
    public string GenerateJwt(User user)
    { 

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


        var tokenHandler = new JwtSecurityTokenHandler();


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            ]),

            Expires = DateTime.UtcNow.AddHours(_expirationToken),

            SigningCredentials = credentials,
            Issuer = _issuer,
            Audience = _audience


        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtString = tokenHandler.WriteToken(token);

        return jwtString;
        
    }

    // Checks if token is valid
    /*
    public ClaimsPrincipal? ValidateJwtToken(string jwt)
    {
        if (string.IsNullOrEmpty(jwt)) return null;

        var tokenHandler = new JwtSecurityTokenHandler();


        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,  
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
            
        };

        return tokenHandler.ValidateToken(jwt, validationParameters, out _);
    }
    */

    // Generate token to send to user for login
    public string GeneratePasswordSetToken()
    {
        // Desired length of token is 10, ratio between bytes and Base64 approximately 3 to 4 (75%)
        var byteCount = (int)Math.Ceiling(10 * 0.75);

        byte[] randomBytes = RandomNumberGenerator.GetBytes(byteCount);

        //Convert random byte array to Base64string
        string token = Convert.ToBase64String(randomBytes);

        // replace 
        token = token.Replace("+", "@").Replace("/", "").Replace("=", "");

        return token;
        
    }

/*    
    public bool ValidatePasswordSetToken(string token)
    {
        return true;   
    }
    */
}
