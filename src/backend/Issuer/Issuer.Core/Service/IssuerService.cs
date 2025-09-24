using System;
using Issuer.API.DTO;
using Issuer.Core.Interfaces;
using Issuer.Core.Interfaces.IService;



namespace Issuer.Core.Service;

public class IssuerService : IIssuerService
{
    private readonly IJwtGenerator _jwtGenerator;

    public IssuerService(IJwtGenerator jwtGenerator)
    {
        _jwtGenerator = jwtGenerator;
    }
    public string CreateCitizen(CitizenDTO citizenDTO)
    {

        if (citizenDTO == null)
        {
            throw new ArgumentNullException("Citizen not passed");
        }
        // Contains all citizen's info
        var citizen = new Citizen(
            citizenDTO.FirstName, citizenDTO.LastName,
            citizenDTO.OtherNames, citizenDTO.NationalIdNumber, citizenDTO.Gender, citizenDTO.DOB,
            citizenDTO.PlaceOfBirth, citizenDTO.Address, citizenDTO.DateOfIssue, citizenDTO.ExpiryDate,
            citizenDTO.PublicKey
        );

        var jwt = _jwtGenerator.GenerateJwt(citizen);  // generates signed jwt


        return jwt;
    }

    //Not implemented for now
    public Task DeleteCitizen(CitizenDTO citizenDTO)
    {
        throw new NotImplementedException();
    }
}
