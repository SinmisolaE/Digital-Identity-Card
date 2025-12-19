using System;
using System.Security.Authentication;
using Issuer.Core.DTO.UserDTO;
using Issuer.Core.Interfaces;
using Issuer.Core.Interfaces.AuthService;
using static Issuer.Core.Data.User;

namespace Issuer.Core.Service.auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHash _passwordHash;
    private readonly ITokenProvider _tokenProvider;

    public AuthService(IUserRepository userRepository, IPasswordHash passwordHash, ITokenProvider tokenProvider)
    {
        _userRepository = userRepository;
        _passwordHash = passwordHash;
        _tokenProvider = tokenProvider;
    }



    public async Task<string?> LoginAsync(UserRequest user)
    {
        // verify details are passed
        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
        {
            throw new ArgumentNullException("Email or password not provided");
        }

        var findUser = await _userRepository.FindUserByEmail(user.Email);

        if (findUser == null) throw new AuthenticationException("Email or password incorrect!");

        if (_passwordHash.VerifyHash(user.Password, findUser.Hashed_Password))
        {
            var jwtString = _tokenProvider.GenerateJwt(findUser);

            return jwtString;
        } else return null;
    }


     public async Task<bool> VerifyTokenAndSetUserPasswordAsync(string email, string token, string newPassword)
    {
        if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email), "Email not provided");
        if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token), "Token not provided");

        var user = await _userRepository.FindUserByEmail(email);

        if (user == null) throw new Exception("User not found");

        // confirm token for user

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
