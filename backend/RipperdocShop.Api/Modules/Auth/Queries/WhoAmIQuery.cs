using System.Security.Claims;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Auth;

namespace RipperdocShop.Api.Modules.Auth.Queries;

public class WhoAmIQuery
{
    public WhoAmIDto Execute(ClaimsPrincipal principal)
    {
        var username = principal.FindFirstValue(ClaimTypes.Name) ?? "Unknown";
        var roles = principal.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        var id = principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Unknown";

        if (!Guid.TryParse(id, out var userId))
            throw new Exception("Not logged in.");

        return new WhoAmIDto
        {
            Id = userId,
            Username = username,
            Roles = roles
        };
    }
}
