using System;
using System.ComponentModel.DataAnnotations;

namespace Issuer.Core.DTO.UserDTO;

public class UserResponse
{
    public UserResponse(string email, string role)
    {
        Email = email;
        Role = role;
    }

    [Required]
    [EmailAddress]
    public string Email {get; set;} = string.Empty;

    [Required]
    public string Role {get; set;} = string.Empty;

    
}
