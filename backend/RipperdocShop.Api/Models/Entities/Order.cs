// OPTIONAL

using RipperdocShop.Api.Models.Identities;

namespace RipperdocShop.Api.Models.Entities;

public enum OrderStatus
{
    Pending,
    Shipping,
    Completed,
    Cancelled,
}

public class Order : ITimestampedEntity
{
    public Guid Id { get; private set; }
    public decimal TotalPrice { get; private set; }
    public OrderStatus Status { get; private set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public Guid UserId { get; private set; }
    public AppUser User { get; private set; } = null!;
    
    public Order() { }
 }
