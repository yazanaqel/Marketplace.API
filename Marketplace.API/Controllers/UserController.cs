using Marketplace.BAL.Services;
using Marketplace.DAL.Dtos.UserDtos;

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

        if (response.Success)
            return Created(response.Message, response.Data);

        return BadRequest(response.Message);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<ServiceResponse<UserResponseDto>>> Login(LoginUserDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _userService.LoginAsync(model);

        if (response.Success)
            return Ok(response);

        return NotFound(response.Message);
    }


}



