using System;
using System.ComponentModel.DataAnnotations;

namespace Verifier.Core.DTO;

public class CitizenDTO
{
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
