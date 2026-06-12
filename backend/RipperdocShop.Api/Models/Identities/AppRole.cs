using Microsoft.AspNetCore.Identity;

namespace RipperdocShop.Api.Models.Identities;

public class AppRole : IdentityRole<Guid>
{
    public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
}
