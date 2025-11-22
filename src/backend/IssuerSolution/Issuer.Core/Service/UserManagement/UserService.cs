using System;
using Issuer.Core.Data;
using Issuer.Core.Interfaces;
using Issuer.Core.Interfaces.AuthService;

namespace Issuer.Core.Service.UserManagement;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IEmailService _emailService;
    private readonly IPasswordHash _passwordHash;

    public UserService(IUserRepository userRepository, ITokenProvider tokenProvider, IEmailService emailService, IPasswordHash passwordHash)
    {
        _userRepository = userRepository;
        _tokenProvider = tokenProvider;
        _emailService = emailService;
        _passwordHash = passwordHash;
    }

    // Create a new User
    public async Task<bool> CreateUserAsync(string email)
    {
        var user = new User(email);

        var token = _tokenProvider.GeneratePasswordSetToken();

        string hashed_token = _passwordHash.HashPassword(token);

        user.AssignToken(token);

        var response = await _userRepository.AddUser(user);

        await _emailService.SendPasswordSetEmailAsync(user.Email, token);

        return true; 


    }

   
    public async Task<bool> DeleteUserAsync(string email)
    {
        if (string.IsNullOrEmpty(email)) throw new Exception("Email not provided");

        try
        {
            
        var response = await _userRepository.DeleteAsync(email);

        return true;
        } catch (Exception ex) { throw new Exception($"Error: {ex.Message}"); }
    }
  
}
