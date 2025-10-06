using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Verifier.Core.DTO;
using Verifier.Core.Interfaces;

namespace Verifier.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifierController : ControllerBase
    {
        private readonly IVerifierService _service;
        private readonly ILogger<VerifierController> _logger;

        public VerifierController(IVerifierService service, ILogger<VerifierController> logger)
        {
            _service = service;

            _logger = logger;
        }

        [HttpPost("/verify")]
        public async Task<ActionResult<CitizenDTO>> VerifyCitizen(JwtDTO jwtDTO)
        {
            _logger.LogInformation("Into verify function");
            try
            {
                _logger.LogInformation("Trying verify service");
                var citizenDTO = await _service.GetCitizenAsync(jwtDTO);

                if (citizenDTO == null) throw new Exception("Citizen not found");

                return Ok(citizenDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error eccured: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
