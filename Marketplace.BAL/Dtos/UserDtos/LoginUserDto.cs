using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.DAL.Dtos.UserDtos;
public class LoginUserDto
{
    [Required]
    public required string Email { get; set; } = string.Empty;

    [Required]
    public required string Password { get; set; } = string.Empty;

    public bool KeepLoggedIn { get; set; }
}
