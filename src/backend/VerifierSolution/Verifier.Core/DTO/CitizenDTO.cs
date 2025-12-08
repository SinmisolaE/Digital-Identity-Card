using System;
using System.ComponentModel.DataAnnotations;
using Verifier.Core.Enitity;

namespace Verifier.Core.DTO;

public class CitizenDTO
{

    public CitizenDTO(string firstName, string lastName, string otherNames, string nationalIdNumber, string photo, string gender, DateOnly dOB, string placeOfBirth, string address, DateOnly dateOfIssue, DateOnly expiryDate)
    {
        FirstName = firstName;
        LastName = lastName;
        OtherNames = otherNames;
        NationalIdNumber = nationalIdNumber;
        Photo = photo;
        Gender = gender;
        DOB = dOB;
        PlaceOfBirth = placeOfBirth;
        Address = address;
        DateOfIssue = dateOfIssue;
        ExpiryDate = expiryDate;
    }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    public string OtherNames { get; set; } = string.Empty;

    [Required]
    public string NationalIdNumber { get; init; } = string.Empty;
    
    [Required]
    public string Photo {get; set;} = string.Empty;
    public string Gender { get; set; } = string.Empty;

    [Required]
    public DateOnly DOB { get; set; }

    [Required]
    public string PlaceOfBirth { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    public DateOnly DateOfIssue { get; set; }

    [Required]
    public DateOnly ExpiryDate { get; set; }
}
