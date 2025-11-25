using Issuer.Core.DTO.UserDTO;
using Issuer.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Issuer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IUserService userService, ILogger<AdminController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateUser(CreateUserRequest user)
        {
            if (user == null || user.Email == null) return BadRequest("Email and role must be provided");

            try
            {
                _logger.LogInformation("Trying to create new user");
                var response = await _userService.CreateUserAsync(user);

                if (response) {
                    return Ok("User Created Successfully!");
                } else
                {
                    return BadRequest("User failed to create!");
                }
            } catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
