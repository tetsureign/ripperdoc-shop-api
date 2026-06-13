using System.ComponentModel.DataAnnotations;
using RipperdocShop.Api.Models.Identities;

namespace RipperdocShop.Api.Models.Entities;

public class ProductRating : ITimestampedEntity, ISoftDeletable
{
    public Guid Id { get; private set; }
    
    [Required]
    [Range(1, 5)]
    public int Score { get; private set; }
    
    [StringLength(3000)]
    public string? Comment { get; private set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    
    public Guid UserId { get; private set; }
    public AppUser User { get; private set; } = null!;
    
    public ProductRating() { }

    public ProductRating(int score, string? comment, Product product, AppUser user)
    {
        if (score is < 1 or > 5)
            throw new ArgumentOutOfRangeException(nameof(score), "Rating must be between 1 and 5.");

        Id = Guid.NewGuid();
        Score = Math.Clamp(score, 1, 5);
        Comment = comment;
        ProductId = product.Id;
        Product = product;
        UserId = user.Id;
        User = user;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    
    public void UpdateDetails(int score, string? comment, Product product, AppUser user)
    {
        if (DeletedAt != null)
            throw new InvalidOperationException("Playing with ghosts, are we? (Cannot update a deleted rating)");

        if (comment != null)
        {
            if (string.IsNullOrWhiteSpace(comment)) 
                throw new ArgumentException("Comments can't be blank or whitespace.");
        }
        
        if (score is < 1 or > 5)
            throw new ArgumentOutOfRangeException(nameof(score), "Rating must be between 1 and 5.");
        
        Score = Math.Clamp(score, 1, 5);
        Comment = comment?.Trim();
        ProductId = product.Id;
        Product = product;
        UserId = user.Id;
        User = user;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SoftDelete()
    {
        if (DeletedAt != null)
            throw new InvalidOperationException("Already flatlined, choom. (Rating is already deleted)");
        
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        if (DeletedAt == null)
            throw new InvalidOperationException("It's still alive, y'know. (Rating is not already deleted)");
        
        DeletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }
}
