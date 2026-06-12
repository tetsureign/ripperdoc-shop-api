namespace RipperdocShop.Shared.DTOs;

public class PaginatedProductResponse
{
    public IEnumerable<ProductDto> Products { get; set; } = null!;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
