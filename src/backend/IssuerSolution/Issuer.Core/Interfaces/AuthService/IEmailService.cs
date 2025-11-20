using System;

namespace Issuer.Core.Interfaces.AuthService;

public interface IEmailService
{
    public Task<bool> SendPasswordSetEmailAsync(string email, string token);


}
