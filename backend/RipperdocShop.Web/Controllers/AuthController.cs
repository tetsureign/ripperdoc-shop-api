using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Web.Models.Auth;
using RipperdocShop.Web.Services;

namespace RipperdocShop.Web.Controllers;

public class AuthController(IUserService userService) : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var dto = new LoginDto
        {
            Email = viewModel.Email,
            Password = viewModel.Password
        };

        var success = await userService.Login(dto.Email, dto.Password);
        if (!success)
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View(viewModel);
        }

        var whoami = await userService.WhoAmI();
        if (whoami is null)
        {
            ModelState.AddModelError("", "Failed to retrieve user data.");
            return View(viewModel);
        }

        // Create identity
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, whoami.Id.ToString()),
            new Claim(ClaimTypes.Name, whoami.Username)
        };
        claims.AddRange(whoami.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await userService.Logout();
        await HttpContext.SignOutAsync(); // Kill the local cookie
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register() {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var dto = new LoginDto
        {
            Email = viewModel.Email,
            Password = viewModel.Password
        };
        var success = await userService.Register(dto.Email, dto.Password);

        if (!success)
        {
            ModelState.AddModelError("", "Registration failed. Email may already be in use.");
            return View(viewModel);
        }

        return RedirectToAction("Login", "Auth");
    }
}
