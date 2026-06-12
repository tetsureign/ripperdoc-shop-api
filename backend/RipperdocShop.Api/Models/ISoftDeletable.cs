namespace RipperdocShop.Api.Models;

public interface ISoftDeletable
{
    DateTime? DeletedAt { get; set; }
 
    void SoftDelete();
    void Restore();
}
