using System;
using System.ComponentModel.DataAnnotations;
using static Issuer.Core.Data.User;

namespace Issuer.Core.DTO.UserDTO;

public class CreateUserRequest
{
    [Required]
    [EmailAddress]
    public string Email {get; set;} = string.Empty;

    [Required]
    public SetRole Role {get; set;}
}
