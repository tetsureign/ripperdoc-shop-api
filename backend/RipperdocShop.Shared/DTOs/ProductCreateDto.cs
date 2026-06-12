namespace RipperdocShop.Shared.DTOs;

public class ProductCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public bool IsFeatured { get; set; } = false;
    public Guid CategoryId { get; set; }

    public Guid? BrandId { get; set; }
}
