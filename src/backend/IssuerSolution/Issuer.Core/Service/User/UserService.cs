using System;
using Issuer.Core.Interfaces;

namespace Issuer.Core.Service.User;

public class UserService : IUserService
{
    public Task<string> CreateUserAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse> LoginAsync(UserRequest user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetUserPasswordAsync(string token, string newPassword)
    {
        throw new NotImplementedException();
    }
}
