using System;

namespace Issuer.Core.Interfaces.AuthService;

public interface IPasswordHash
{
    public string HashPassword(string password);

    public bool VerifyHash(string password, string hashed_password);
}
