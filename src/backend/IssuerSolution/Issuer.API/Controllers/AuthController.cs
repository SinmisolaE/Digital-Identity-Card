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

        [HttpPost]
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
    }
}
