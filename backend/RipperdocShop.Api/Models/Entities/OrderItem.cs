// OPTIONAL

namespace RipperdocShop.Api.Models.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public string ProductNameSnapshot { get; private set; } = string.Empty;
    public string ProductSlugSnapshot { get; private set; } = string.Empty;
    public string ProductDescriptionSnapshot { get; private set; } = string.Empty;
    public string ProductImageUrlSnapshot { get; private set; } = string.Empty;
    public string ProductCategorySnapshot { get; private set; } = string.Empty;
    public string? ProductBrandSnapshot { get; private set; } = string.Empty;
    public decimal ProductPriceSnapshot { get; private set; }
    public int Quantity { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;
    
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    
    public OrderItem() { }
}
