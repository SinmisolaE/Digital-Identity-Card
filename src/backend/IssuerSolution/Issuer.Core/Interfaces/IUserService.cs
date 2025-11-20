using System;

namespace Issuer.Core.Interfaces;

public interface IUserService
{
    public Task<string> CreateUserAsync(string email);

    public Task<bool> SetUserPasswordAsync(string token, string newPassword);

    public Task<UserResponse> LoginAsync(UserRequest user);
}
