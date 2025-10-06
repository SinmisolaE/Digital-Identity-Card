using System;
using System.ComponentModel.DataAnnotations;
using Verifier.Core.Enitity;

namespace Verifier.Core.DTO;

public class CitizenDTO
{
    private Citizen citizen;

    public CitizenDTO(Citizen citizen)
    {
        this.citizen = citizen;
    }

    public CitizenDTO(string firstName, string lastName, string otherNames, string nationalIdNumber, string gender, DateOnly dOB, string placeOfBirth, string address, DateOnly dateOfIssue, DateOnly expiryDate, string publicKey)
    {
        FirstName = firstName;
        LastName = lastName;
        OtherNames = otherNames;
        NationalIdNumber = nationalIdNumber;
        Gender = gender;
        DOB = dOB;
        PlaceOfBirth = placeOfBirth;
        Address = address;
        DateOfIssue = dateOfIssue;
        ExpiryDate = expiryDate;
        PublicKey = publicKey;
    }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    public string OtherNames { get; set; } = string.Empty;

    [Required]
    public string NationalIdNumber { get; init; } = string.Empty;

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

    public string PublicKey { get; init; } = string.Empty;
}
