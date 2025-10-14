using System.Text.Json.Nodes;
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
        private readonly INonceService _nonceService;

        private readonly ILogger<VerifierController> _logger;

        public VerifierController(IVerifierService service, ILogger<VerifierController> logger, INonceService nonceService)
        {
            _service = service;
            _nonceService = nonceService;


            _logger = logger;
        }

        [HttpPost("/verify")]
        public async Task<ActionResult<CitizenDTO>> VerifyCitizen(JwtDTO jwtDTO)
        {
            _logger.LogInformation("Into verify function");
            try
            {
                _logger.LogInformation("Ensure nonce challenge is correct");
                var response = _nonceService.IsValid(jwtDTO.nonce);

                if (!response)
                {
                    return BadRequest("Connection requirements not met for secure communication");
                }

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

        [HttpGet("/challenge")]
        public ActionResult<string> GetChallengeAndVerifyUrl()
        {
            _logger.LogInformation("To get the nonce challenge");

            try
            {
                var nonce = _nonceService.GenerateNonce();

                var verificationUrl = _service.GetUrl();

                return Ok(new
                {
                    success = true,
                    nonce = nonce,
                    verificationUrl = verificationUrl
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }
        

    }
}
