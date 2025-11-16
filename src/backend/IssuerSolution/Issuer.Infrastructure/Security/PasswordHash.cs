using System;
using BCrypt.Net;
//using BCrypt = BCrypt.Net.BCrypt;
using Issuer.Core.Interfaces.AuthService;
using Microsoft.Extensions.Logging;

namespace Issuer.Infrastructure.Security;

public class PasswordHash : IPasswordHash
{
    private readonly int _workFactor = 12;

    private readonly ILogger<PasswordHash> _logger;

    public PasswordHash(ILogger<PasswordHash> logger)
    {
        _logger = logger;
    }


    // Hashes password
    public string HashPassword(string password)
    {
        _logger.LogInformation("Into hash password function");
        if (string.IsNullOrEmpty(password)) {
            _logger.LogError("Password not provided");
            throw new ArgumentException("Password not provided");
        }

        _logger.LogInformation("Hashing password");
        return BCrypt.Net.BCrypt.HashPassword(password, _workFactor);

    }

    // Verify provided password matches hashes
    public bool VerifyHash(string password, string hashed_password)
    {
        _logger.LogInformation("Into verify hash password function");
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashed_password))
        {
            _logger.LogError("Password not provided");    
            return false;
        }

        try
        {
            _logger.LogInformation("Verifying password against hashed");

            return BCrypt.Net.BCrypt.Verify(password, hashed_password);
        } catch (BcryptAuthenticationException e)
        {
            _logger.LogError($"Error when verifying password with hash: {e.Message}");
            
            return false;
        }
    }
}
