// OPTIONAL

using RipperdocShop.Api.Models.Identities;

namespace RipperdocShop.Api.Models.Entities;

public class CartItem
{
    public Guid Id { get; private set; }
    public int Quantity { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public Guid UserId { get; private set; }
    public AppUser User { get; private set; } = null!;
    
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    
    public CartItem() { }
}
