using System;
using Issuer.Core.DTO.UserDTO;
using Issuer.Core.Interfaces;
using Issuer.Core.Interfaces.AuthService;

namespace Issuer.Core.Service.auth;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHash _passwordHash;

    public AuthService(IUserRepository userRepository, IPasswordHash passwordHash)
    {
        _userRepository = userRepository;
        _passwordHash = passwordHash;
    }



    public async Task<bool> LoginAsync(UserRequest user)
    {
        // verify details are passed
        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
        {
            throw new ArgumentNullException("Email or password not provided");
        }

        var find_user = await _userRepository.FindUserByEmail(user.Email);

        if (find_user == null) throw new Exception("User doesn't exist");

        if (_passwordHash.VerifyHash(user.Password, find_user.Hashed_Password))
        {
            return true;
        } else return false;
    }

      public Task<bool> SetUserPasswordAsync(string token, string newPassword)
    {
        throw new NotImplementedException();
    }
}
