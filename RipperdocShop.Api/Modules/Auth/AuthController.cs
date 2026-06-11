using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Api.Models.Identities;

namespace RipperdocShop.Api.Modules.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController(
    SignInManager<AppUser> signInManager,
    UserManager<AppUser> userManager,
    JwtService jwt,
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
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user is not { DeletedAt: null } || user.IsDisabled)
            return Unauthorized("User doesn't exist or access revoked");

        var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            return Unauthorized("Wrong creds, choom");

        var roles = await userManager.GetRolesAsync(user);
        // if (!roles.Contains("Admin"))
        //     return Unauthorized("Looking at the wrong place, choom");
        
        var token = jwt.GenerateToken(user, roles);
        
        Response.Cookies.Append("AccessToken", token, GetCookieOptions());
        
        // Return the token straight to the response on dev env to make it easier to test with Swagger
        return env.IsDevelopment() 
            ? Ok(new { message = "Access granted", token }) 
            : Ok(new { message = "Access granted" });
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginDto dto)
    {
        var user = new AppUser { UserName = dto.Email, Email = dto.Email };

        var result = await userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        await userManager.AddToRoleAsync(user, "Customer");

        var token = jwt.GenerateToken(user, ["Customer"]);
        
        Response.Cookies.Append("AccessToken", token, GetCookieOptions());
        
        // Return the token straight to the response on dev env to make it easier to test with Swagger
        return env.IsDevelopment() 
            ? Ok(new { message = "Customer account created", token }) 
            : Ok(new { message = "Customer account created" });
    }
    
    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AccessToken", GetCookieOptions());
        return Ok(new { message = "Wiped clean" });
    }
    
    [HttpGet("whoami")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult WhoAmI()
    {
        var username = User.FindFirstValue(ClaimTypes.Name) ?? "Unknown";
        var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Unknown";

        if (!Guid.TryParse(id, out var userId))
            throw new Exception("Not logged in.");

        return Ok(new WhoAmIDto
        {
            Id = userId,
            Username = username,
            Roles = roles
        });
    }
}
