using System;
using Issuer.API.DTO;
using Issuer.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Issuer.Core.Service;

public class IssuerService : IIssuerService
{
    private readonly IJwtGenerator _jwtGenerator;

    private readonly ILogger<IssuerService> _logger;

    public IssuerService(IJwtGenerator jwtGenerator, ILogger<IssuerService> logger)
    {
        _jwtGenerator = jwtGenerator;
        _logger = logger;
    }

    public async Task<string> CreateCitizenAsync(CitizenDTO citizenDTO)
    {
        _logger.LogInformation("Started creating citizen digital card");

        if (citizenDTO == null)
        {
            _logger.LogError("Citizen not passed");
            throw new ArgumentNullException("Citizen not passed");
        }
        // Contains all citizen's info
        var citizen = new Citizen(
            citizenDTO.FirstName, citizenDTO.LastName,
            citizenDTO.OtherNames, citizenDTO.NationalIdNumber, citizenDTO.Gender, citizenDTO.DOB,
            citizenDTO.PlaceOfBirth, citizenDTO.Address, citizenDTO.DateOfIssue, citizenDTO.ExpiryDate
        );

        var jwt = await _jwtGenerator.GenerateJwtAsync(citizen);  // generates signed jwt


        return jwt;
    }

    //Not implemented for now
    public Task DeleteCitizen(CitizenDTO citizenDTO)
    {
        throw new NotImplementedException();
    }
}
