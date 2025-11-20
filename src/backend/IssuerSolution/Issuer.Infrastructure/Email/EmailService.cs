using System;
using Issuer.Core.Interfaces.AuthService;

namespace Issuer.Infrastructure.Email;

public class EmailService : IEmailService
{
    public async Task<bool> SendPasswordSetEmailAsync(string email, string token)
    {
        
        return true;
    }
}
