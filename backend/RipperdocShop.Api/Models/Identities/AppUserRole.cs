using Microsoft.AspNetCore.Identity;

namespace RipperdocShop.Api.Models.Identities;

public class AppUserRole : IdentityUserRole<Guid>
{
    public AppUser User { get; set; }
    public AppRole Role { get; set; }
}
