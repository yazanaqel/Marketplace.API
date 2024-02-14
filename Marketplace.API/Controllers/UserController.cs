using Marketplace.BAL.Services;
using Marketplace.BAL.Dtos.UserDtos;

namespace Marketplace.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost("Register")]
    public async Task<ActionResult<ServiceResponse<UserResponseDto>>> Register(RegisterUserDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _userService.RegisterAsync(model);

        return response.Success ? Created(response.Message, response) : BadRequest(response.Message);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<ServiceResponse<UserResponseDto>>> Login(LoginUserDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _userService.LoginAsync(model);

        return response.Success ? Ok(response) : NotFound(response.Message);
    }


}



