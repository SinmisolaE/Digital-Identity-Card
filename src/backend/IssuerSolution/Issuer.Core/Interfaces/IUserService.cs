using System;
using Issuer.Core.DTO.UserDTO;

namespace Issuer.Core.Interfaces;

public interface IUserService
{
    Task<bool> CreateUserAsync(CreateUserRequest user);
    Task<bool> DeleteUserAsync(string email);


    
}
