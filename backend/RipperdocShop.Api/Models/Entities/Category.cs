using System.ComponentModel.DataAnnotations;
using RipperdocShop.Api.Utils;

namespace RipperdocShop.Api.Models.Entities;

public class Category : ITimestampedEntity, ISoftDeletable
{
    public Guid Id { get; private set; }

    [Required] [StringLength(100)] public string Name { get; private set; } = string.Empty;

    [Required] [StringLength(120)] public string Slug { get; private set; } = string.Empty;

    [Required] [StringLength(1000)] public string Description { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    private Category()
    {
    }

    public Category(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("It's got a name y'know (Category name is required)");

        Id = Guid.NewGuid();
        Name = name.Trim();
        Slug = SlugGenerator.GenerateSlug(name);
        Description = description.Trim();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string description)
    {
        if (DeletedAt != null)
            throw new InvalidOperationException("Playing with ghosts, are we? (Cannot update a deleted category)");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("It's got a name y'know (Category name is required)");

        Name = name.Trim();
        Slug = SlugGenerator.GenerateSlug(name);
        Description = description.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        if (DeletedAt != null)
            throw new InvalidOperationException("Already flatlined, choom. (Category is already deleted)");

        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        if (DeletedAt == null)
            throw new InvalidOperationException("It's still alive, y'know. (Category is not already deleted)");

        DeletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }
}
