namespace RipperdocShop.Shared.DTOs;

public class ProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsFeatured { get; set; } = false;
    public CategoryDto Category { get; set; } = null!;
    public BrandCreateDto? Brand { get; set; }
}
