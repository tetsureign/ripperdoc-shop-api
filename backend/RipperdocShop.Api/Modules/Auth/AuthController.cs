using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Api.Modules.Auth.Commands;
using RipperdocShop.Api.Modules.Auth.Queries;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Auth;

namespace RipperdocShop.Api.Modules.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController(
    LoginCommand loginCommand,
    RegisterCommand registerCommand,
    LogoutCommand logoutCommand,
    WhoAmIQuery whoAmI,
    IHostEnvironment env)
    : ControllerBase
{
    private CookieOptions GetCookieOptions()
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = env.IsDevelopment() ? SameSiteMode.None : SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddHours(2)
            // Path = "/",
        };
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await loginCommand.ExecuteAsync(dto);
        if (!result.Succeeded)
            return Unauthorized(result.Message);

        Response.Cookies.Append("AccessToken", result.Token!, GetCookieOptions());

        // Return the token straight to the response on dev env to make it easier to test with Swagger
        return env.IsDevelopment() 
            ? Ok(new { message = result.Message, token = result.Token }) 
            : Ok(new { message = result.Message });
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginDto dto)
    {
        var result = await registerCommand.ExecuteAsync(dto);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        Response.Cookies.Append("AccessToken", result.Token!, GetCookieOptions());

        // Return the token straight to the response on dev env to make it easier to test with Swagger
        return env.IsDevelopment() 
            ? Ok(new { message = result.Message, token = result.Token }) 
            : Ok(new { message = result.Message });
    }
    
    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AccessToken", GetCookieOptions());
        return Ok(logoutCommand.Execute());
    }
    
    [HttpGet("whoami")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult WhoAmI()
    {
        return Ok(whoAmI.Execute(User));
    }
}
