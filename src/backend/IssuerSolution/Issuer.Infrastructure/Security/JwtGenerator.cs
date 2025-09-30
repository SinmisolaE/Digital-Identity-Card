using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using Issuer.Core;
using Issuer.Core.Interfaces;
using Issuer.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Issuer.Infrastructure;

public class JwtGenerator : IJwtGenerator
{
    private readonly RsaSecurityKey _privateKey;

    private readonly string _privateKeyPem;

    private readonly string _publicKeyPem;
    private readonly ILogger<JwtGenerator> _logger;


    public JwtGenerator(IRsaKeyService rsa, ILogger<JwtGenerator> logger)
    {
        _privateKey = rsa.GetPrivateKey();
        _privateKeyPem = rsa.GetPrivateKeyPem();
        _publicKeyPem = rsa.GetPublicKeyPem();
        _logger = logger;
    }

    // Generate jwt for citizen details
    public string GenerateJwt(Citizen citizen)
    {
        _logger.LogInformation("Trying to generate jwt");
        var tokenHandler = new JwtSecurityTokenHandler();



        System.Console.WriteLine();
        System.Console.WriteLine($"Private: {_privateKeyPem}");
        System.Console.WriteLine($"Public: {_publicKeyPem}");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim("FirstName", citizen.FirstName),
                new Claim("LastName", citizen.LastName),
                new Claim("OtherNames", citizen.OtherNames),
                new Claim("NationalID", citizen.NationalIdNumber),
                new Claim("DOB", citizen.DOB.ToString()),
                new Claim("Address", citizen.Address),
                new Claim("Gender", citizen.Gender),
                new Claim("DateOfIssue", citizen.DateOfIssue.ToString()),
                new Claim("ExpiryDate", citizen.ExpiryDate.ToString()),
                new Claim("PlaceOfBirth", citizen.PlaceOfBirth),
                new Claim("CitizenPublicKey", citizen.PublicKey)
            }),

            Expires = DateTime.UtcNow.AddYears(10),



            SigningCredentials = new SigningCredentials(_privateKey, SecurityAlgorithms.RsaSha256)


        };

        var jwt = tokenHandler.CreateToken(tokenDescriptor);
        var jwtString = tokenHandler.WriteToken(jwt);

        _logger.LogInformation("JWT generated");

        return jwtString;
    }
}
