using Marketplace.BAL.DbContext;
using Marketplace.DAL;
using Marketplace.DAL.Dtos.ProductDtos;
using Marketplace.DAL.Dtos.UserDtos;
using Marketplace.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Marketplace.BAL.Services.UserService;
public class UserService(UserManager<ApplicationUser> userManager, IOptions<HelperJWT> helperJWT) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly HelperJWT _helperJWT = helperJWT.Value;

    public async Task<ServiceResponse<UserResponseDto>> RegisterAsync(RegisterUserDto model)
    {
        ServiceResponse<UserResponseDto> serviceResponse = new ServiceResponse<UserResponseDto>();

        try
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                serviceResponse.Message = CustomConstants.User.EmailIsTaken;
                return serviceResponse;
            }

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }

                serviceResponse.Message = errors;
                return serviceResponse;
            }

            var jwtToken = await CreateJwtToken(user);
            var stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            serviceResponse.Data = new UserResponseDto { JWT = stringToken, ExpiresOn = jwtToken.ValidTo, Email = model.Email };

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
        }

        serviceResponse.Message = CustomConstants.User.UserCreated;
        serviceResponse.Success = true;

        return serviceResponse;

    }

    public async Task<ServiceResponse<UserResponseDto>> LoginAsync(LoginUserDto model)
    {
        ServiceResponse<UserResponseDto> serviceResponse = new ServiceResponse<UserResponseDto>();

        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                serviceResponse.Message = CustomConstants.User.Incorrect;
                return serviceResponse;
            }

            var jwtToken = await CreateJwtToken(user);
            var stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            serviceResponse.Data = new UserResponseDto { JWT = stringToken, ExpiresOn = jwtToken.ValidTo, Email = model.Email };
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
        }

        serviceResponse.Message = CustomConstants.User.UserLogin;
        serviceResponse.Success = true;

        return serviceResponse;
    }

    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        try
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            }
            .Union(userClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_helperJWT.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _helperJWT.Issuer,
                audience: _helperJWT.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_helperJWT.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
}
