using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using RipperdocShop.Api.Utils;

namespace RipperdocShop.Api.Models.Entities;

public class Brand : ITimestampedEntity, ISoftDeletable
{
    public Guid Id { get; private set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; private set; } = string.Empty;
    
    [Required]
    [StringLength(120)]
    public string Slug { get; private set; } = string.Empty;
    
    [Required]
    [StringLength(1000)]
    public string Description { get; private set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public Brand() { }

    public Brand(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Arasaka might find you for this. (Brand name is required)");
        
        Id = Guid.NewGuid();
        Name = name.Trim();
        Slug = SlugGenerator.GenerateSlug(name);
        Description = description.Trim();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Arasaka might find you for this. (Brand name is required)");
        if (DeletedAt != null)
            throw new InvalidOperationException("Playing with ghosts, are we? (Cannot update a deleted brand)");

        Name = name.Trim();
        Slug = SlugGenerator.GenerateSlug(name);
        Description = description.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SoftDelete()
    {if (DeletedAt != null)
            throw new InvalidOperationException("Already flatlined, choom. (Brand is already deleted)");
        
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        if (DeletedAt == null)
            throw new InvalidOperationException("It's still alive, y'know. (Brand is not already deleted)");
        
        DeletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }
}
