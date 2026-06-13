using Microsoft.AspNetCore.Identity;
using RipperdocShop.Api.Models.Identities;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Auth;

namespace RipperdocShop.Api.Modules.Auth.Commands;

public class RegisterCommand(UserManager<AppUser> userManager, JwtService jwt)
{
    public async Task<AuthCommandResult> ExecuteAsync(LoginDto dto)
    {
        var user = new AppUser { UserName = dto.Email, Email = dto.Email };

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return new AuthCommandResult(
                false,
                "Could not create customer account",
                FailureReason: AuthFailureReason.IdentityErrors,
                Errors: result.Errors);

        await userManager.AddToRoleAsync(user, "Customer");

        var token = jwt.GenerateToken(user, ["Customer"]);
        return new AuthCommandResult(true, "Customer account created", token);
    }
}
