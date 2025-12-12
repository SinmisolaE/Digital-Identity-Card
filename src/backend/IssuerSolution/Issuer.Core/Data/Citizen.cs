using System;
using System.ComponentModel.DataAnnotations;

namespace Issuer.Core;


// This class takes the credentials of citizen 
public class Citizen
{
    public Citizen(string firstName, string lastName, string otherNames, string nationalIdNumber, string photo, string gender, DateOnly dOB, string placeOfBirth, string address, DateOnly dateOfIssue, DateOnly expiryDate)
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
    public string FirstName { get; private set; } = string.Empty;

    [Required]
    public string LastName { get; private set; } = string.Empty;

    public string OtherNames { get; private set; } = string.Empty;

    [Required]
    public string NationalIdNumber { get; init; } = string.Empty;

    [Required]
    public string Photo { get; set; } = string.Empty;

    [Required]
    public string Gender { get; private set; } = string.Empty;

    [Required]
    public DateOnly DOB { get; private set; }

    [Required]
    public string PlaceOfBirth { get; private set; } = string.Empty;

    [Required]
    public string Address { get; private set; } = string.Empty;

    [Required]
    public DateOnly DateOfIssue { get; private set; }

    [Required]
    public DateOnly ExpiryDate { get; private set; }

    
}
