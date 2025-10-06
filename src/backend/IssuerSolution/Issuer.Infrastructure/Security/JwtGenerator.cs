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
    private readonly IRsaKeyService _rsa;

    //Using Lazy initialization
    private readonly Lazy<RsaSecurityKey> _privateKey;

    private readonly Lazy<string> _privateKeyPem;

    private readonly Lazy<Task<string>> _publicKeyPem;
    private readonly ILogger<JwtGenerator> _logger;


    public JwtGenerator(IRsaKeyService rsa, ILogger<JwtGenerator> logger)
    {
        _rsa = rsa;

        _privateKey = new Lazy<RsaSecurityKey>(() => rsa.GetPrivateKey());
        _privateKeyPem = new Lazy<string>(() => rsa.GetPrivateKeyPem());
        _publicKeyPem = new Lazy<Task<string>>(async () => await rsa.GetPublicKeyPem());
        _logger = logger;
    }

    // Generate jwt for citizen details
    public async Task<string> GenerateJwtAsync(Citizen citizen)
    {
        _logger.LogInformation("Trying to generate jwt");
        var tokenHandler = new JwtSecurityTokenHandler();

        var publicKey = await _publicKeyPem.Value;

        System.Console.WriteLine();
        System.Console.WriteLine($"Private: {_privateKeyPem.Value}");
        System.Console.WriteLine();
        System.Console.WriteLine();

        System.Console.WriteLine($"Public: {publicKey}");

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



            SigningCredentials = new SigningCredentials(_privateKey.Value, SecurityAlgorithms.RsaSha256)


        };

        var jwt = tokenHandler.CreateToken(tokenDescriptor);
        var jwtString = tokenHandler.WriteToken(jwt);

        _logger.LogInformation("JWT generated");

        return jwtString;
    }
}
