using System;
using System.ComponentModel.DataAnnotations;

namespace Issuer.Core.DTO.UserDTO;

public class CreateUserRequest
{
    [Required]
    [EmailAddress]
    public string Email {get; set;} = string.Empty;
}
