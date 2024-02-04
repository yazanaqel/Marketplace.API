using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.DAL.Dtos.UserDtos;
public class UserResponseDto
{
    public required string Email { get; set; } = string.Empty;
    public required string JWT { get; set; } = string.Empty;
    public required DateTime ExpiresOn { get; set; }

}
