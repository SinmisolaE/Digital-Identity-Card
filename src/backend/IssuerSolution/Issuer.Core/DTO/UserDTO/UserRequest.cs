using System;
using System.ComponentModel.DataAnnotations;

namespace Issuer.Core.DTO.UserDTO;

public class UserRequest
{
    [Required]
    public string Email {get; set;} = string.Empty;

    [Required]
    public string Password {get; set;} = string.Empty;

}
