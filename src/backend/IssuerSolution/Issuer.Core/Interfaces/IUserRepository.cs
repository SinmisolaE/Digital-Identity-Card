using System;
using Issuer.Core.Data;

namespace Issuer.Core.Interfaces;

public interface IUserRepository
{
    Task<bool> AddUser(User user);

    Task<User> FindUserByEmail(string email);

    Task<bool> DeleteAsync(string email);
}
