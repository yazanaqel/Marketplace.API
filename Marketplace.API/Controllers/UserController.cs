using Marketplace.BAL.Services.UserService;
using Marketplace.DAL;
using Marketplace.DAL.Dtos.UserDtos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            return Ok(response);

        return BadRequest(response.Message);
    }
    
    [HttpPost("Login")]
    public async Task<ActionResult<ServiceResponse<UserResponseDto>>> Login(LoginUserDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _userService.LoginAsync(model);

        if(response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }


}



