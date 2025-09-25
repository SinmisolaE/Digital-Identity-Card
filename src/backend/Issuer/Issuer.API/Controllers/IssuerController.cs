using Issuer.API.DTO;
using Issuer.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Issuer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuerController : ControllerBase
    {
        private readonly IIssuerService _issuerService;
        private readonly ILogger<IssuerController> _logger;

        public IssuerController(IIssuerService issuerService, ILogger<IssuerController> logger)
        {
            _issuerService = issuerService;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<string> CreateCitizen(CitizenDTO citizenDTO)
        {
            try
            {
                _logger.LogInformation("Trying to create citizen's digital version");
                if (citizenDTO == null)
                {
                    _logger.LogWarning("Citizen data not passed");
                    throw new ArgumentNullException("Citizen's details not provided");
                }

                string jwt = _issuerService.CreateCitizen(citizenDTO);

                _logger.LogInformation("jwt is being sent");


                return Ok(jwt);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Errorrr: {e.Message}");
                return BadRequest("Error:" + e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Errorrr: {e.Message}");

                return BadRequest("Error: " + e.Message);
            }
            
        }
    }
}
