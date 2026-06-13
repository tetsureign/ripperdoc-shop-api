using Microsoft.AspNetCore.Identity;
using RipperdocShop.Api.Models.Identities;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Auth;

namespace RipperdocShop.Api.Modules.Auth.Commands;

public class LoginCommand(
    SignInManager<AppUser> signInManager,
    UserManager<AppUser> userManager,
    JwtService jwt)
{
    public async Task<AuthCommandResult> ExecuteAsync(LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user is not { DeletedAt: null } || user.IsDisabled)
            return new AuthCommandResult(
                false,
                "User doesn't exist or access revoked",
                FailureReason: AuthFailureReason.UserMissingOrRevoked);

        var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            return new AuthCommandResult(
                false,
                "Wrong creds, choom",
                FailureReason: AuthFailureReason.WrongCredentials);

        var roles = await userManager.GetRolesAsync(user);
        var token = jwt.GenerateToken(user, roles);

        return new AuthCommandResult(true, "Access granted", token);
    }
}
