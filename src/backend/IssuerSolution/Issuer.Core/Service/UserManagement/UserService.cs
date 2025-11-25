using System;
using Issuer.Core.Data;
using Issuer.Core.DTO.UserDTO;
using Issuer.Core.Events;
using Issuer.Core.Interfaces;
using Issuer.Core.Interfaces.AuthService;
using Issuer.Core.Interfaces.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Issuer.Core.Service.UserManagement;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenProvider _tokenProvider;
    private readonly IEmailService _emailService;
    private readonly IPasswordHash _passwordHash;
    private readonly IOutBoxService _outBoxService;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, ITokenProvider tokenProvider, IEmailService emailService, IPasswordHash passwordHash, IOutBoxService outBoxService, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenProvider = tokenProvider;
        _emailService = emailService;
        _passwordHash = passwordHash;
        _outBoxService = outBoxService;
        _logger = logger;
    }

    // Create a new User
    public async Task<bool> CreateUserAsync(CreateUserRequest user)
    {
        var new_user = new User(user.Email, user.Role);

        _logger.LogInformation("Generating password set token");
        var token = _tokenProvider.GeneratePasswordSetToken();

        string hashed_token = _passwordHash.HashPassword(token);

        new_user.AssignToken(token);

        _logger.LogInformation("Creating user to db");
        var response = await _userRepository.AddUser(new_user);

        //create event
        _logger.LogInformation("Creating user event");
        var userEvent = new UserCreatedEvent(new_user.Id, new_user.Email, new_user.ResetPasswordToken);

        // save event
        await _outBoxService.SaveEventAsync(userEvent);

        // save with a single write
        await _unitOfWork.SaveEntitiesAsync();

        _logger.LogInformation("User saved successfully");

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
