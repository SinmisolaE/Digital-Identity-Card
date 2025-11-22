using System;

namespace Issuer.Core.Interfaces;

public interface IUserService
{
    public Task<bool> CreateUserAsync(string email);

    
}
