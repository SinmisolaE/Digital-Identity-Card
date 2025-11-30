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

/*
  This JWT contains the citizen details (name, IDNumber, etc)
  It uses an rsa key pair: 
   private key signs the jwt - used by issuer
   public key verifies the jwt - used by verifiers

 */
public class JwtGenerator : IJwtGenerator
{
    private readonly IRsaKeyService _rsa;


    private readonly ILogger<JwtGenerator> _logger;


    public JwtGenerator(IRsaKeyService rsa, ILogger<JwtGenerator> logger)
    {
        _rsa = rsa;

        _logger = logger;
    }

    // Generate jwt for citizen details signed with issuer's private key 
    public async Task<string> GenerateJwtAsync(Citizen citizen)
    {
        _logger.LogInformation("Trying to generate jwt");
        var tokenHandler = new JwtSecurityTokenHandler();

        _logger.LogInformation("Getting private key");
        var privateKey = await _rsa.GetPrivateKey();

        System.Console.WriteLine();
        System.Console.WriteLine();
        System.Console.WriteLine();

        //System.Console.WriteLine($"Public: {publicKey}");

        _logger.LogInformation("Generating JWT signed by private key generated");
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
                new Claim("Photo", citizen.Photo),
                new Claim("DateOfIssue", citizen.DateOfIssue.ToString()),
                new Claim("ExpiryDate", citizen.ExpiryDate.ToString()),
                new Claim("PlaceOfBirth", citizen.PlaceOfBirth)
            }),

            Expires = DateTime.UtcNow.AddYears(10),



            SigningCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256)


        };

        var jwt = tokenHandler.CreateToken(tokenDescriptor);
        var jwtString = tokenHandler.WriteToken(jwt);

        _logger.LogInformation("JWT generated");

        return jwtString;
    }
}
