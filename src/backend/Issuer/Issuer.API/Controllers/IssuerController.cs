using Issuer.API.DTO;
using Issuer.Core.Interfaces.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Issuer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuerController : ControllerBase
    {
        private readonly IIssuerService _issuerService;

        public IssuerController(IIssuerService issuerService)
        {
            _issuerService = issuerService;
        }

        [HttpPost]
        public ActionResult<string> CreateCitizen(CitizenDTO citizenDTO)
        {
            try
            {
                if (citizenDTO == null)
                {
                    throw new ArgumentNullException("Citizen's details not provided");
                }

                string jwt = _issuerService.CreateCitizen(citizenDTO);

                return Ok(200);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest("Error:" + e.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Error: " + e.Message);
            }
            
        }
    }
}
