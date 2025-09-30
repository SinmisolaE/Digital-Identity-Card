using System.Data.SqlTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using TrustRegistryService.Core.DTO;
using TrustRegistryService.Core.Services;

namespace TrustRegistryService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistryController : ControllerBase
    {
        private readonly IRegistryService _registryService;

        public RegistryController(IRegistryService registryService)
        {
            _registryService = registryService;
        }

        [HttpGet("/issuer")]
        public async Task<ActionResult<RegistryDTO>> GetRegistryByIssuerAsync(string issuer)
        {
            try
            {
                if (string.IsNullOrEmpty(issuer))
                {
                    throw new ArgumentNullException("Issuer is not passed");
                }
                var registryDTO = await _registryService.GetRegistryByIssuerAsync(issuer);

                if (registryDTO == null)
                {
                    throw new SqlNullValueException("Registry not found");
                }

                return Ok(registryDTO);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("/register")]
        public async Task<ActionResult<bool>> AddRegistryAsync(RegistryDTO registryDTO)
        {
            if (registryDTO == null) {
                return BadRequest("Details not provided");
            }
            try
            {
                var response = await _registryService.AddRegistryAsync(registryDTO);

                if (!response)
                {
                    throw new Exception("Error Occured: Registry not added");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
