using System;
using Issuer.Core.DTO.UserDTO;

namespace Issuer.Core.Interfaces;

public interface IAuthService
{
    Task<UserResponse?> LoginAsync(UserRequest user);
    Task<bool> VerifyTokenAndSetUserPasswordAsync(string email, string token, string newPassword);
}
