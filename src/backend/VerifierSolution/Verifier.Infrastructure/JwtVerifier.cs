using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Verifier.Core.DTO;
using Verifier.Core.Enitity;
using Verifier.Core.Interfaces;


namespace Verifier.Infrastructure;

public class JwtVerifier : IJwtVerifier
{
    //private readonly RSA _publicKey;
    private readonly ILogger<JwtVerifier> _logger;

    public JwtVerifier(ILogger<JwtVerifier> logger)
    {
        _logger = logger;
        
    }

    public Citizen ValidateAndExtractCitizen(string jwt, string publicKeyPem)
    {
        _logger.LogInformation("Into validate and extract jwt");

        var (header, payload, signature) = ParseJwt(jwt);

        RSA publicKey = LoadPublicKey(publicKeyPem);

        if (!VerifySignature(header, payload, signature, publicKey))
            throw new Exception("Invalid JWT signature");

        var claims = ExtractClaims(payload);

        return CreateCitizenFromClaims(claims);
    }

    private (string, string, string) ParseJwt(string jwt)
    {
        _logger.LogInformation("Splitting jwt");
        var parts = jwt.Split('.');

        if (parts.Length != 3) throw new Exception("Invalid JWT structure");

        return (parts[0], parts[1], parts[2]);
    }

    private bool VerifySignature(string header, string payload, string signature, RSA publicKey)
    {
        _logger.LogInformation("Verifying signature");
        string signingInput = $"{header}.{payload}";

        byte[] signingInputBytes = Encoding.UTF8.GetBytes(signingInput);

        byte[] signatureByte = DecodeBase64UrlToBytes(signature);

        return publicKey.VerifyData(
            signingInputBytes,
            signatureByte,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1
        );
    }

    private Dictionary<string, object> ExtractClaims(string payload)
    {
        string payloadJson = DecodeBase64Url(payload);

        return JsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);
    }


    private Citizen CreateCitizenFromClaims(Dictionary<string, object> claims)
    {
        _logger.LogInformation("Creating citizen from claims");

        return new Citizen(
            GetClaimValue(claims, "FirstName"),
            GetClaimValue(claims, "LastName"),
            GetClaimValue(claims, "OtherNames"),
            GetClaimValue(claims, "NationalID"),
            GetClaimValue(claims, "Gender"),
            DateOnly.Parse(GetClaimValue(claims, "DOB")),
            GetClaimValue(claims, "PlaceOfBirth"),
            GetClaimValue(claims, "Address"),

            DateOnly.Parse(GetClaimValue(claims, "DateOfIssue")),
            DateOnly.Parse(GetClaimValue(claims, "ExpiryDate")),

            GetClaimValue(claims, "CitizenPublicKey")

        );
    }

    private string GetClaimValue(Dictionary<string, object> claims, string key)
    {
        return claims.ContainsKey(key) ? claims[key]?.ToString() : null;
    }

    private string DecodeBase64Url(string base64Url)
    {
        string base64 = base64Url.Replace('-', '+').Replace('_', '/');

        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;

            case 3: base64 += "="; break;
        }

        byte[] bytes = Convert.FromBase64String(base64);

        return Encoding.UTF8.GetString(bytes);
    }

    private byte[] DecodeBase64UrlToBytes(string base64Url)
    {
        string base64 = base64Url.Replace('-', '+').Replace('_', '/');

        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;

            case 3: base64 += "="; break;
        }

        return Convert.FromBase64String(base64);
    }

    private RSA LoadPublicKey(string publicKeyPem)
    {
        _logger.LogInformation("Loading public key");
        

        RSA rsa = RSA.Create();
        rsa.ImportFromPem(publicKeyPem);

        //rsa.ImportFromPem("-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2ZEy49hl+qHTCoe0FJvT\nQ9PEevCw/4jJ93H9I0jVw1KQLpVFwkuakVT5WMKd7/mRmci4w03BMHmeqkDclkiT\nyApvFGHn0ebc2MT4/qLydwu+nQ09uYOWDchBkcC29UC4UjAl0rcozupEZ3SMd+PB\n11JMUTPW54S58WDHPVoUFau8jvOMXjgeNlqCakfdGevFzlVYLtTGCbRTtve9DoBJ\n8UZr84yi0JeeRWO7aIsmRkAk7bOlw71JAWrlBge4ghdHH+gMVUcba3kYDOdnRXTW\nZoxZcE+HILWTAuMVZsvAcx10patIBttISYoz3jf5wX2TO9O0x8cHLUzaf+JMOSrS\nMQIDAQAB\n-----END PUBLIC KEY-----");


        return rsa;
    }
}
