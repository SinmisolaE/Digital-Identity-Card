using Issuer.Core.DTO.UserDTO;
using Issuer.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Issuer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("/login")]
        public async Task<ActionResult<UserResponse>> Login(UserRequest user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Provide both email and password!");
            }

            try
            {
                
                var response = await _authService.LoginAsync(user);

                if (response == null)
                {
                    return BadRequest("Email or password incorrect!");
                }
                return new UserResponse(response.Email, response.Role);
            } catch (Exception e) {
                return BadRequest($"Error: {e.Message}");
            }
        }
    
        // Verify user's token and Update user's password
        [HttpPost("/reset-password")]
        public async Task<ActionResult<bool>> UpdatePassword(UserPasswordChange userPasswordChange)
        { 
            if (userPasswordChange == null || string.IsNullOrEmpty(userPasswordChange.Email) || string.IsNullOrEmpty(userPasswordChange.Password)
                || string.IsNullOrEmpty(userPasswordChange.Token))
            {
                _logger.LogInformation("Not all parameters provided");
                return BadRequest("Email and Token must be provided");
            }
            _logger.LogInformation($"Into password change for {userPasswordChange.Email}");

            try
            {
                if (await _authService.VerifyTokenAndSetUserPasswordAsync(userPasswordChange.Email, userPasswordChange.Token, userPasswordChange.Password))
                {
                    _logger.LogInformation($"Password set successfully for {userPasswordChange.Email} : {userPasswordChange.Password}");
                    return Ok("Password reset successfull");
                }
                return BadRequest("Token Invalid!");
            } catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return BadRequest(ex.Message);
            }


            
        }
    }
}
