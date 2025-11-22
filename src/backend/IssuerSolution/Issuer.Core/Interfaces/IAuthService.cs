using System;
using Issuer.Core.DTO.UserDTO;

namespace Issuer.Core.Interfaces;

public interface IAuthService
{
    Task<UserResponse> LoginAsync(UserRequest user);
    Task<bool> SetUserPasswordAsync(string token, string newPassword);
}
