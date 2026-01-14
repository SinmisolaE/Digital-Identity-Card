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

        var Photo = await UploadPhoto(citizenDTO.NationalIdNumber, citizenDTO.Photo);

        // Contains all citizen's info
        var citizen = new Citizen(
            citizenDTO.FirstName, citizenDTO.LastName,
            citizenDTO.OtherNames, citizenDTO.NationalIdNumber, Photo, citizenDTO.Gender, citizenDTO.DOB,
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



    // PhotoService
    public async Task<string> UploadPhoto(string NationalIdNumber, string Photo)
    {
        try
        {
            
            // Extract base64 data (remove "data:image/jpeg;base64," prefix if present)
            var base64Data = Photo;
            if (base64Data.Contains(","))
            {
                base64Data = base64Data.Split(',')[1];
            }
            
            // Convert to bytes
            var photoBytes = Convert.FromBase64String(base64Data);
            
            // Ensure directory exists
            var photosDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos");
            Directory.CreateDirectory(photosDir);
            
            // Save file
            var filename = $"{NationalIdNumber}_{Guid.NewGuid():N}.jpg";
            var filePath = Path.Combine(photosDir, filename);
            await File.WriteAllBytesAsync(filePath, photoBytes);
                
            // Return URL
            var photoUrl = $"/api/photos/{filename}";
            return photoUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload photo for citizen {NationalId}", NationalIdNumber);
            throw new InvalidOperationException($"Failed to save photo: {ex.Message}");
        }
        
    }

}
