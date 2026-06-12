using Microsoft.AspNetCore.Identity;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Models.Identities;

public sealed class AppUser : IdentityUser<Guid>, ITimestampedEntity, ISoftDeletable
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDisabled { get; private set; }
    public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();

    // Warning: EF Core will use this too.
    public AppUser()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public AppUser(string email)
    {
        UserName = email;
        Email = email;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        if (DeletedAt != null)
            throw new InvalidOperationException("Already flatlined, choom. (User is already deleted)");

        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        if (DeletedAt == null)
            throw new InvalidOperationException("They're still alive, y'know. (User is not deleted)");

        DeletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Disable()
    {
        if (IsDisabled)
            throw new InvalidOperationException("Nah, already banned. (User is already disabled.)");

        IsDisabled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Enable()
    {
        if (!IsDisabled)
            throw new InvalidOperationException("They're still active. (User is not disabled.)");

        IsDisabled = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
