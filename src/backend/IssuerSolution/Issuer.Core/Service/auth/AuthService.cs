using System;
using Issuer.Core.DTO.UserDTO;
using Issuer.Core.Interfaces;
using Issuer.Core.Interfaces.AuthService;
using static Issuer.Core.Data.User;

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



    public async Task<UserResponse?> LoginAsync(UserRequest user)
    {
        // verify details are passed
        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
        {
            throw new ArgumentNullException("Email or password not provided");
        }

        var findUser = await _userRepository.FindUserByEmail(user.Email);

        if (findUser == null) throw new Exception("User doesn't exist");

        if (_passwordHash.VerifyHash(user.Password, findUser.Hashed_Password))
        {
            return new UserResponse(findUser.Email, Enum.GetName(typeof(SetRole), findUser.Role));
        } else return null;
    }

    // confirm token for user
    public async Task<bool> ConfirmTokenAsync(string email, string token)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token)) throw new ArgumentNullException("Provide all arguments");

        var user = await _userRepository.FindUserByEmail(email);

        if (user == null) throw new Exception("User not found");

        if (_passwordHash.VerifyHash(token, user.ResetPasswordToken))
        {
            if(DateTime.UtcNow < user.TokenExpiry)
                return true;
        }
        return false;
    }

     public async Task<bool> SetUserPasswordAsync(string email, string token, string newPassword)
    {
        if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email), "Email not provided");
        if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token), "Token not provided");
        var user = await _userRepository.FindUserByEmail(email);

        if (user == null) throw new Exception("User not found");

        if (_passwordHash.VerifyHash(token, user.ResetPasswordToken))
        {
            if(DateTime.UtcNow < user.TokenExpiry)
            {
                var hashed_password = _passwordHash.HashPassword(newPassword);

                await _userRepository.UpdatePasswordAsync(user, hashed_password);

                await _userRepository.SaveChangesAsync();

                return true;
            }
        }
        return false;
    }
}
