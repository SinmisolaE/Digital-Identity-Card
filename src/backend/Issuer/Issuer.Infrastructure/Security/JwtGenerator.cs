using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using Issuer.Core;
using Issuer.Core.Interfaces;
using Issuer.Infrastructure.Data;
using Microsoft.IdentityModel.Tokens;

namespace Issuer.Infrastructure;

public class JwtGenerator : IJwtGenerator
{
    ILogger _lo
    public IRsaKeyService _rsa;

    public JwtGenerator(IRsaKeyService rsa)
    {
        _rsa = rsa;
    }

    public string GenerateJwt(Citizen citizen)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var privateKey = _rsa.PrivateKey();

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
                new Claim("PlaceOfBirth", citizen.PlaceOfBirth)
            }),

            Expires = DateTime.UtcNow.AddYears(10),



            SigningCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256)


        };

        var jwt = tokenHandler.CreateToken(tokenDescriptor);
        var jwtString = tokenHandler.WriteToken(jwt);

        return jwtString;
    }
}
