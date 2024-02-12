using Marketplace.BAL.Dtos.UserDtos;

namespace Marketplace.BAL.Services.UserService;
public interface IUserService
{
    Task<ServiceResponse<UserResponseDto>> LoginAsync(LoginUserDto model);
    Task<ServiceResponse<UserResponseDto>> RegisterAsync(RegisterUserDto model);
}